using Lilac.Combat;
using Lilac.Components;
using Lilac.Components.Controllers;
using Lilac.Dice;

namespace Lilac.Entities.Creatures;

public sealed class GiantRat : WildBeast
{
	public GiantRat(int level)
	 : base("Giant Rat", level, 5, 10)
	{
		if (GetComponent<StatsComponent>() is { } stats)
		{
			stats.Strength = 6;
			stats.Agility = 10;
			stats.Intelligence = 2;
			stats.Constitution = 4;
			stats.Perception = 12;
			stats.Charisma = 1;
		}

		if (GetComponent<CombatComponent>() is not { } combatComponent)
			return;

		combatComponent.battleState.AttackAttribute = StatsComponent.Attribute.Agility;
		combatComponent.battleState.DamageType = DamageType.Piercing;
		combatComponent.battleState.DamageRoll = Roll.Die.D4 + (1 + (level - 1) * 2);
	}

	public override void Render()
	{
		Render(Name, Level);
	}
}