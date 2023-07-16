using System;
using System.Collections.Generic;
using Lilac.Dice;
using Lilac.Combat;

namespace Lilac.Components;

public sealed class CombatComponent : IComponent
{
	public BattleState battleState;
	
	private readonly StatsComponent statsComponent;

	public CombatComponent(StatsComponent statsComponent)
	{
		this.statsComponent = statsComponent;
	}

	/// <summary>The defense bonus to all physical damage.</summary>
	public int Defense { get; set; }
	
	/// <summary>The resistance bonus to all magical damage.</summary>
	public int Resistance { get; set; }

	/// <summary>The defense bonuses to specific physical damage types.</summary>
	public Dictionary<DamageType.Physical, int> Defenses { get; } = new();

	/// <summary>The resistance bonuses to specific magical damage types.</summary>
	public Dictionary<DamageType.Magical, int> Resistances { get; } = new();

	/// <summary>Returns the final defense for the given <see cref="DamageType.Physical"/>.</summary>
	public int GetDefense(DamageType.Physical damageType)
	{
		var defenseBonus = 0;
		foreach (var bonus in statsComponent.bonuses)
		{
			defenseBonus += bonus.Defense;
		}
		
		return defenseBonus + Defense + (Defenses.TryGetValue(damageType, out var defense) ? defense : 0);
	}

	/// <summary>Returns the final resistance for the given <see cref="DamageType.Magical"/>.</summary>
	public int GetResistance(DamageType.Magical damageType)
	{
		return Resistance + (Resistances.TryGetValue(damageType, out var resistance) ? resistance : 0);
	}

	/// <summary>Returns the final defense/resistance for the given <see cref="DamageType"/>.</summary>
	public int GetArmor(DamageType damageType)
	{
		switch (damageType)
		{
			case DamageType.Physical physicalDamageType:
				return GetDefense(physicalDamageType);
			case DamageType.Magical magicalDamageType:
				return GetResistance(magicalDamageType);
			default:
				return 0;
		}
	}
	
	public Roll Initiative
	{
		get
		{
			var initiativeBonus = 0;
			foreach (var bonus in statsComponent.bonuses)
			{
				initiativeBonus += bonus.Initiative;
			}
			
			return Roll.Die.D20 + ((statsComponent.Agility + statsComponent.Perception) / 4 + initiativeBonus);
		}
	}
	public int Evasion => 10 + statsComponent.Agility / 2;

	public AttackResult RollAttack(CombatComponent? targetComponent)
	{
		var advantage = battleState.AttackAdvantage - (targetComponent?.battleState.DefenseAdvantage ?? 0);
		var baseDie = Roll.Die.D20;

		if (Game.Singleton?.CurrentDifficulty == Game.Difficulty.Easy)
			advantage++;
		
		var rollCount = (uint)(Math.Abs(advantage) + 1);
		
		Roll baseRoll = advantage switch
		{
			> 0 => new Roll.KeepHighest(rollCount, 1, baseDie),
			< 0 => new Roll.KeepLowest(rollCount, 1, baseDie),
			_   => baseDie
		};

		var attackRoll = baseRoll + statsComponent.GetAttribute(battleState.AttackAttribute) / 2 + battleState.HitBonus;
		var attackRollResult = attackRoll.Execute();

		var criticalState = CriticalState.Normal;
		if (attackRollResult is DieRollResult dieRollResult)
			criticalState = GetCriticalState(dieRollResult.RollValue, battleState.CriticalChanceBonus);

		return new AttackResult(attackRollResult.Value, criticalState);
	}

	public CheckResult RollCheck(StatsComponent.Attribute attribute, int bonus = 0, int advantage = 0)
	{
		var baseDie = Roll.Die.D20;

		var rollCount = (uint)(Math.Abs(advantage) + 1);
		
		Roll baseRoll = advantage switch
		{
			> 0 => new Roll.KeepHighest(rollCount, 1, baseDie),
			< 0 => new Roll.KeepLowest(rollCount, 1, baseDie),
			_   => baseDie
		};
		
		var checkRoll = baseRoll + statsComponent.GetAttribute(attribute) / 2 + bonus;
		var checkRollResult = checkRoll.Execute();

		var criticalState = CriticalState.Normal;
		if (checkRollResult is DieRollResult dieRollResult)
			criticalState = GetCriticalState(dieRollResult.RollValue);

		return new CheckResult(checkRollResult.Value, criticalState);
	}

	public int RollDamage(bool critical)
	{
		if (battleState.DamageRoll is null)
			return 0;

		var damage = battleState.DamageRoll.Execute().Value;

		if (critical)
		{
			var maximum = battleState.DamageRoll.BaseRange().Item2;
			damage += maximum.Value * (1 + battleState.CriticalDamageBonus);
		}
		
		return damage;
	}

	private CriticalState GetCriticalState(int rollValue, int criticalChanceBonus = 0)
	{
		var criticalThreshold = 20 - criticalChanceBonus;
		if (rollValue >= criticalThreshold)
			return CriticalState.CriticalSuccess;

		if (rollValue <= 1)
			return CriticalState.CriticalFailure;

		return CriticalState.Normal;
	}

	public DamageSource ReceiveDamage(DamageSource source, int minimum = 1)
	{
		var armor = GetArmor(source.Type);
		var newDamage = System.Math.Max(minimum, source.Damage - armor);
		return source with { Damage = newDamage };
	}

	public int GetInitiative() => battleState.Initiative;

	public struct BattleState
	{
		public Roll? DamageRoll { get; set; }
		public DamageType? DamageType { get; set; }
		public StatsComponent.Attribute AttackAttribute { get; set; }
		public int HitBonus { get; set; }
		public int Initiative { get; set; }
		public bool Hidden { get; set; }
		public int AttackAdvantage { get; set; }
		public int DefenseAdvantage { get; set; }
		public int CriticalChanceBonus { get; set; }
		public int CriticalDamageBonus { get; set; }
	}
	
	public enum CriticalState : byte
	{
		Normal,
		CriticalSuccess,
		CriticalFailure
	}

	public readonly struct AttackResult
	{
		public AttackResult(int attackValue, CriticalState criticalState)
		{
			AttackValue = attackValue;
			CriticalState = criticalState;
		}
		
		public int AttackValue { get; }
		public CriticalState CriticalState { get; }

		public static readonly AttackResult Miss = new(0, CriticalState.CriticalFailure);
	}

	public readonly struct CheckResult
	{
		public CheckResult(int checkValue, CriticalState criticalState)
		{
			CheckValue = checkValue;
			CriticalState = criticalState;
		}
		
		public int CheckValue { get; }
		public CriticalState CriticalState { get; }
	}
}