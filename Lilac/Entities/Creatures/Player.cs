using System;
using Lilac.Combat;
using Lilac.Components;
using Lilac.Components.Controllers;
using Lilac.Dice;
using Lilac.Items;
using Attribute = Lilac.Combat.Attribute;

namespace Lilac.Entities.Creatures;

public sealed class Player : Creature
{
	public Player(Character character)
		: base(character.Race.DisplayName)
	{
		Name = character.Name;
		AddComponent(new UserController(this));

		var equipmentComponent = new EquipmentComponent();
		AddComponent(equipmentComponent);

		var inventoryComponent = new InventoryComponent(equipmentComponent);
		AddComponent(inventoryComponent);

		AddComponent(new CharacterComponent(character, GetComponent<StatsComponent>() ??
													   throw new Exception("Player requires a StatsComponent.")));

		JoinAllegiance(Allegiance.Player);

		if (character.StartingWeapon is not null)
		{
			var startingWeapon = new WeaponInstance(character.StartingWeapon, Material.Bronze);
			inventoryComponent.AddItem(startingWeapon);
			equipmentComponent.Equip(startingWeapon);
		}

		var startingArmor = character.StartingArmor switch
		{
			Armor.MetalArmor   => new ArmorInstance(character.StartingArmor, Material.Bronze),
			Armor.LeatherArmor => new ArmorInstance(character.StartingArmor, Material.Padded),
			_                  => null
		};

		if (startingArmor is not null)
		{
			inventoryComponent.AddItem(startingArmor);
			equipmentComponent.Equip(startingArmor);
		}

		UpdateCombatStats();

		if (GetComponent<CombatComponent>() is not { } combatComponent)
			return;

		combatComponent.equipmentComponent = equipmentComponent;
		combatComponent.Affinities = character.Affinities;
		equipmentComponent.OnEquipmentChanged += UpdateCombatStats;
	}

	public static Attribute DefaultAttribute => Attribute.Strength;
	public static DamageType DefaultDamageType => DamageType.Crushing;
	public static Roll DefaultDamageRoll => Roll.Die.D4;

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

		combatComponent.battleState.AttackAttribute = equipmentComponent?.Weapon?.AttackAttribute ?? DefaultAttribute;
		combatComponent.battleState.DamageType = equipmentComponent?.Weapon?.DamageType ?? DefaultDamageType;
		combatComponent.battleState.DamageRoll = equipmentComponent?.Weapon?.DamageRoll ?? DefaultDamageRoll;
	}
}