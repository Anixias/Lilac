using System;
using Lilac.Combat;
using Lilac.Components;
using Lilac.Components.Controllers;
using Lilac.Dice;
using Attribute = Lilac.Combat.Attribute;

namespace Lilac.Entities.Creatures;

public sealed class Player : Creature
{
	public Player(Character character) : base(character.Race.DisplayName)
	{
		Name = character.Name;
		AddComponent(new UserController(this));
		AddComponent(new CharacterComponent(character, GetComponent<StatsComponent>() ??
													   throw new Exception("Player requires a StatsComponent.")));

		JoinAllegiance(Allegiance.Player);

		if (GetComponent<CombatComponent>() is not { } combatComponent)
			return;

		combatComponent.Affinities = character.Affinities;
		combatComponent.battleState.AttackAttribute = Attribute.Strength;
		combatComponent.battleState.DamageType = DamageType.Slashing;
		combatComponent.battleState.DamageRoll = Roll.Die.D6;
	}

	public Character Character => GetComponent<CharacterComponent>()?.Character ??
								  throw new Exception("Missing Character for Player.");

	public override void Render()
	{
		if (GetComponent<CharacterComponent>() is not { } characterComponent)
			return;

		characterComponent.Character.Display();
	}
}