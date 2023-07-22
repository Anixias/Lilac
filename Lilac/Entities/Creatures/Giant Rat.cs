using Lilac.Combat;
using Lilac.Components;
using Lilac.Components.Controllers;
using Lilac.Dice;

namespace Lilac.Entities.Creatures;

public sealed class GiantRat : Creature
{
	public GiantRat(int level = 1) : base("Giant Rat", level)
	{
		AddComponent(new AIController(this));
		AddComponent(new RewardComponent
		{
			XP = 10 * level
		});

		var stats = GetComponent<StatsComponent>();
		if (stats is not null)
		{
			stats.Strength = 6;
			stats.Agility = 10;
			stats.Intelligence = 2;
			stats.Constitution = 4;
			stats.Perception = 12;
			stats.Charisma = 1;
		}

		JoinAllegiance(Allegiance.Wild);
		JoinAllegiance(Allegiance.Aggressive);

		if (GetComponent<CombatComponent>() is not { } combatComponent)
			return;

		combatComponent.battleState.AttackAttribute = Attribute.Agility;
		combatComponent.battleState.DamageType = DamageType.Piercing;
		combatComponent.battleState.DamageRoll = Roll.Die.D4 + (1 + (level - 1) * 2);
	}

	public override void Render()
	{
		Render(Name);
	}
}