using System;
using System.Collections.Generic;
using Lilac.Components;
using Lilac.Components.Controllers;
using Lilac.Combat;
using Lilac.Rendering;

namespace Lilac.Entities.Creatures;

public abstract class Creature : Entity, IHittable, IBattleMember, IAttacker
{
    private readonly List<StatusEffect> statusEffects = new();
    private readonly Dictionary<IRelationship, RelationshipState> relationships = new();
    private readonly List<Allegiance> allegiances = new();
    private RelationshipState defaultRelationship = RelationshipState.Neutral;

    public event IBattleMember.EventHandler? OnEndTurn;

    protected Creature(string species)
    {
        Name = species;
        Species = species;

        var healthComponent = new HealthComponent();
        var manaComponent = new ManaComponent();
        var levelComponent = new LevelComponent();
        var statsComponent = new StatsComponent(healthComponent, manaComponent, levelComponent);
        var combatComponent = new CombatComponent(statsComponent);

        AddComponent(healthComponent);
        AddComponent(manaComponent);
        AddComponent(levelComponent);
        AddComponent(statsComponent);
        AddComponent(combatComponent);
    }

    protected Creature(string species, int level)
    : this(species)
    {
        var levelComponent = GetComponent<LevelComponent>();
        if (levelComponent is null)
            return;

        levelComponent.XP = LevelComponent.GetXPFromLevel(level);
    }

    public RelationshipState DefaultRelationship
    {
        get => defaultRelationship;
        init => defaultRelationship = value;
    }

    public RelationshipState? InboundDefaultRelationship { get; init; }

    public string Name { get; init; }

    public bool IsDead => Health <= 0;
    public Allegiance[] Allegiances => allegiances.ToArray();
    public int Health => GetComponent<HealthComponent>()?.Health ?? 0;
    public int MaxHealth => GetComponent<HealthComponent>()?.MaxHealth ?? 0;
    public int Mana => GetComponent<ManaComponent>()?.Mana ?? 0;
    public int MaxMana => GetComponent<ManaComponent>()?.MaxMana ?? 0;
    public int Level => GetComponent<LevelComponent>()?.Level ?? 1;
    public int XP => GetComponent<LevelComponent>()?.XP ?? 0;
    public int MaxXP => LevelComponent.GetXPFromLevel(Level + 1);
    public int Initiative => GetComponent<CombatComponent>()?.battleState.Initiative ?? 0;
    public int Evasion => GetComponent<CombatComponent>()?.Evasion ?? 0;
	public bool IsUser => GetComponent<IController>()?.IsUser ?? false;
    public string Species { get; }

    protected void Render(string name)
    {
        Screen.Write(name);
    }

    /// <summary>Prepares this <see cref="Creature"/> for battle by rolling Initiative, setting up Stealth parameters, and more.</summary>
    public void PrepareForBattle(Battle battle)
    {
        if (GetComponent<CombatComponent>() is not { } combatComponent)
            return;

        RollInitiative();
        combatComponent.battleState.Hidden = false;
        combatComponent.battleState.AttackAdvantage = 0;

		if (GetComponent<IController>() is not { } controller)
			return;

		controller.BeginBattle(battle);
    }

	public void TakeTurn()
	{
		if (GetComponent<IController>() is not { } controller)
			return;

		controller.TakeTurn();
	}

    private void RollInitiative()
    {
        if (GetComponent<CombatComponent>() is not { } combatComponent)
            return;

        combatComponent.battleState.Initiative = combatComponent.Initiative.Execute().Value;
    }

    public int GetInitiative()
    {
        return GetComponent<CombatComponent>()?.GetInitiative() ?? 0;
    }

    /// <summary>Attempts an attack against the target <see cref="IHittable"/> using current combat parameters.</summary>
    /// <returns><see langword="true"/> if the attack hits the target; <see langword="false"/> otherwise.</returns>
    public bool Attack(IHittable target)
    {
        if (GetComponent<CombatComponent>() is not { } combatComponent)
            return false;

        CombatComponent? targetCombatComponent = null;
        if (target is Entity targetEntity)
            targetCombatComponent = targetEntity.GetComponent<CombatComponent>();

        var attackRollResult = combatComponent.RollAttack(targetCombatComponent);
        var criticalHit = false;
        var hit = false;

        switch (attackRollResult.CriticalState)
        {
            case CombatComponent.CriticalState.CriticalSuccess:
                hit = true;
                criticalHit = true;
                break;
            case CombatComponent.CriticalState.Normal:
                hit = target.CheckHit(attackRollResult.AttackValue);
                break;
        }

        if (!hit)
            return false;

        var damageRollResult = combatComponent.RollDamage(criticalHit);
        var damageType = combatComponent.battleState.DamageType ?? DamageType.Raw;
        target.ReceiveDamage(damageType.CreateDamage(damageRollResult));
        return true;
    }

    public void ReceiveDamage(DamageSource source)
    {
        if (GetComponent<HealthComponent>() is not { } healthComponent)
            return;

        if (GetComponent<CombatComponent>() is { } combatComponent)
        {
            var minimumDamage = 1;
            if (IsUser)
            {
                minimumDamage = Game.Singleton?.CurrentDifficulty switch
                {
                    // Armor can completely negate damage
                    Game.Difficulty.Easy   => 0,
                    
                    // Armor can negate all but X damage
                    Game.Difficulty.Normal => 1,
                    Game.Difficulty.Hard   => 2,
                    _                      => 1
                };

                minimumDamage = Math.Min(minimumDamage, source.Damage);
            }
            
            source = combatComponent.ReceiveDamage(source, minimumDamage);
        }

        healthComponent.Health -= source.Damage;
    }

    public bool CheckHit(int attackValue)
    {
        if (GetComponent<CombatComponent>() is not { } combatComponent)
            return false;

        return attackValue >= combatComponent.Evasion;
    }

    public void EndTurn()
    {
        OnEndTurn?.Invoke();
    }

    public void TurnStarted()
    {
        foreach (var effect in statusEffects)
        {
            effect.OnTurnStarted();
        }
    }

    public void TurnChanged()
    {
        foreach (var effect in statusEffects)
        {
            effect.OnTurnChanged();
        }
    }

    public void TurnEnded()
    {
        foreach (var effect in statusEffects)
        {
            effect.OnTurnEnded();
        }
    }

    protected void JoinAllegiance(Allegiance allegiance)
    {
        allegiances.Add(allegiance);

		defaultRelationship = (RelationshipState)Math.Min((int)defaultRelationship, (int)allegiance.DefaultRelationship);
    }

    protected void LeaveAllegiance(Allegiance allegiance)
    {
        allegiances.Remove(allegiance);

		RelationshipState? newDefaultRelationship = null;
		foreach (var existingAllegiance in allegiances)
		{
			if (newDefaultRelationship is null)
			{
				newDefaultRelationship = existingAllegiance.DefaultRelationship;
				continue;
			}

			newDefaultRelationship = (RelationshipState)Math.Min((int)newDefaultRelationship, (int)allegiance.DefaultRelationship);
		}

		defaultRelationship = newDefaultRelationship ?? RelationshipState.Neutral;
    }

    public RelationshipState GetRelationship(IRelationship? allegiance)
    {
        if (allegiance is null)
            return defaultRelationship;

        if (allegiance is Allegiance allegianceLocal && this.allegiances.Contains(allegianceLocal))
            return RelationshipState.Ally;

		if (relationships.TryGetValue(allegiance, out var relationship))
			return relationship;

		if (allegiance.InboundDefaultRelationship is { } inboundRelationship)
			return inboundRelationship;

		return defaultRelationship;
    }

    public RelationshipState GetRelationship(IRelationship[] allegiances)
    {
        // Returns the lowest allegiance found, or default if none found
        RelationshipState? relationship = null;
        RelationshipState? lowestInbound = null;

        foreach (var allegiance in allegiances)
        {
            if (allegiance is Allegiance allegianceLocal && this.allegiances.Contains(allegianceLocal))
            {
                if (relationship is not null)
                    relationship = (RelationshipState)Math.Min((int)relationship, (int)RelationshipState.Ally);
                else relationship = RelationshipState.Ally;

                continue;
            }

            if (relationships.TryGetValue(allegiance, out var allegianceRelationship))
            {
                if (relationship is not null)
                    relationship = (RelationshipState)Math.Min((int)relationship, (int)allegianceRelationship);
                else relationship = allegianceRelationship;
            }

			if (allegiance.InboundDefaultRelationship is { } inbound)
			{
				if (lowestInbound is not null)
					lowestInbound = (RelationshipState)Math.Min((int)lowestInbound, (int)inbound);
				else lowestInbound = inbound;
			}
        }

        return relationship ?? lowestInbound ?? defaultRelationship;
    }

    public void SetRelationship(IRelationship? allegiance, RelationshipState relationship)
    {
        if (allegiance is null)
        {
            defaultRelationship = relationship;
            return;
        }

        relationships[allegiance] = relationship;
    }

    public void RemoveRelationship(IRelationship allegiance)
    {
        if (relationships.ContainsKey(allegiance))
            relationships.Remove(allegiance);
    }
}