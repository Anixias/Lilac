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
		var equipmentComponent = new EquipmentComponent();
		AddComponent(equipmentComponent);
		AddComponent(new InventoryComponent(equipmentComponent));
		AddComponent(new CharacterComponent(character, GetComponent<StatsComponent>() ??
													   throw new Exception("Player requires a StatsComponent.")));

		JoinAllegiance(Allegiance.Player);
		UpdateCombatStats();

		if (GetComponent<CombatComponent>() is not { } combatComponent)
			return;

		combatComponent.Affinities = character.Affinities;
		equipmentComponent.OnEquipmentChanged += UpdateCombatStats;
	}

	public Character Character => GetComponent<CharacterComponent>()?.Character ??
								  throw new Exception("Missing Character for Player.");

	public override void Render()
	{
		if (GetComponent<CharacterComponent>() is not { } characterComponent)
			return;

		characterComponent.Character.Display();
	}

	private void UpdateCombatStats()
	{
		if (GetComponent<CombatComponent>() is not { } combatComponent)
			return;

		var equipmentComponent = GetComponent<EquipmentComponent>();

		combatComponent.battleState.AttackAttribute =
			equipmentComponent?.Weapon?.AttackAttribute ?? Attribute.Strength;
		combatComponent.battleState.DamageType = equipmentComponent?.Weapon?.DamageType ?? DamageType.Crushing;
		combatComponent.battleState.DamageRoll = equipmentComponent?.Weapon?.DamageRoll ?? Roll.Die.D4;
	}
}