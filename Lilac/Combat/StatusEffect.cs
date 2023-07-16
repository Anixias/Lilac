using Lilac.Components;
using Lilac.Entities;

namespace Lilac.Combat;

public enum StatusEffectAlignment : byte
{
	Negative,
	Positive,
	Neutral
}

/// <summary>
/// An object that represents a prolonged effect affecting an <see cref="Entity"/>.
/// </summary>
public abstract class StatusEffect
{
	private StatusEffect(Entity entity)
	{
		Entity = entity;
	}
	
	private Entity Entity { get; }
	public abstract StatusEffectAlignment Alignment { get; }
	public abstract string DisplayIcon { get; }
	public abstract string DisplayName { get; }
	public abstract string Description { get; }
	public abstract int Duration { get; }
	public int TurnsLeft { get; set; }
	
	/// <summary>Called at the start of the affected creature's turn.</summary>
	public virtual void OnTurnStarted() {}

	/// <summary>Called at the end of the affected creature's turn.</summary>
	public virtual void OnTurnEnded() {}

	/// <summary>Called at the start of the any creature's turn, including the affected creature.</summary>
	public virtual void OnTurnChanged() {}

	/// <summary>Called when the effect is first inflicted.</summary>
	public virtual void OnInflicted() {}

	/// <summary>Called when the effect expires.</summary>
	public virtual void OnExpired() {}

	/// <summary>A <see cref="StatusEffect"/> that triggers and then expires instantly.</summary>
	public abstract class InstantStatusEffect : StatusEffect
	{
		public InstantStatusEffect(Entity entity)
		: base(entity)
		{
		}
		
		public sealed override int Duration => 0;
	
		public sealed override void OnTurnStarted() {}
		public sealed override void OnTurnEnded() {}
		public sealed override void OnTurnChanged() {}
		public sealed override void OnExpired() {}
	}

	public sealed class Evading : StatusEffect
	{
		public Evading(Entity entity)
			: base(entity)
		{
			
		}

		public override StatusEffectAlignment Alignment => StatusEffectAlignment.Positive;
		public override string DisplayIcon => "(Ev)";
		public override string DisplayName => "Evading";

		public override string Description => "The affected creature is focusing on avoiding attacks, granting -1 " +
											  "Advantage to all attacks received until the start of its next turn.";

		public override int Duration => 1;

		public override void OnInflicted()
		{
			if (Entity.GetComponent<CombatComponent>() is { } combatComponent)
				combatComponent.battleState.DefenseAdvantage += 1;
		}

		public override void OnExpired()
		{
			if (Entity.GetComponent<CombatComponent>() is { } combatComponent)
				combatComponent.battleState.DefenseAdvantage -= 1;
		}
	}

	public sealed class Bleeding : StatusEffect
    {
		public Bleeding(Entity entity) 
			: base(entity)
		{
		}
		
        public override StatusEffectAlignment Alignment => StatusEffectAlignment.Negative;
        public override string DisplayIcon => "(Bl)";
        public override string DisplayName => "Bleeding";
        public override string Description => "The affected creature is receiving 1 raw damage at the start of its turns.";
        public override int Duration => 3;

		public override void OnTurnStarted()
		{
			if (Entity is IHittable hittable)
				hittable.ReceiveDamage(DamageType.Raw.CreateDamage(1));
		}
    }

    public sealed class Stunned : StatusEffect
    {
		public Stunned(Entity entity)
		: base(entity)
		{
		}
		
        public override StatusEffectAlignment Alignment => StatusEffectAlignment.Negative;
        public override string DisplayIcon => "(St)";
        public override string DisplayName => "Stunned";
        public override string Description => "The affected creature is losing its turns.";
        public override int Duration => 2;

		public override void OnTurnStarted()
		{
			if (Entity is IBattleMember battleMember)
				battleMember.EndTurn();
		}
    }
}