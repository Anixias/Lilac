using Lilac.Items;

namespace Lilac.Components;

public sealed class EquipmentComponent : IComponent
{
	public delegate void EventHandler();

	private ItemInstance? amulet;
	private ItemInstance? armor;
	private WeaponInstance? weapon;

	public WeaponInstance? Weapon
	{
		get => weapon;
		private set
		{
			weapon = value;
			OnEquipmentChanged?.Invoke();
		}
	}

	public ItemInstance? Armor
	{
		get => armor;
		private set
		{
			armor = value;
			OnEquipmentChanged?.Invoke();
		}
	}

	public ItemInstance? Amulet
	{
		get => amulet;
		private set
		{
			amulet = value;
			OnEquipmentChanged?.Invoke();
		}
	}

	public event EventHandler? OnEquipmentChanged;

	/// <summary>
	///     Returns whether the given item is equipped or not.
	/// </summary>
	/// <param name="item">The item to check the equipped status of.</param>
	/// <returns><see langword="true" /> if the item is equipped; <see langword="false" /> otherwise.</returns>
	public bool IsEquipped(ItemInstance item)
	{
		if (Weapon == item)
			return true;

		if (Armor == item)
			return true;

		if (Amulet == item)
			return true;

		return false;
	}

	/// <summary>
	///     Unequips the given <see cref="ItemInstance" /> if it is currently equipped.
	/// </summary>
	/// <param name="item">The item to unequip.</param>
	public void Unequip(ItemInstance item)
	{
		if (Weapon == item)
			Weapon = null;

		if (Armor == item)
			Armor = null;

		if (Amulet == item)
			Amulet = null;
	}

	/// <summary>
	///     Equips the given <see cref="ItemInstance" /> if it can be equipped.
	/// </summary>
	/// <param name="item">The item to equip.</param>
	public void Equip(ItemInstance item)
	{
		switch (item.Framework.EquipmentSlot)
		{
			case EquipmentSlot.Weapon:
				Weapon = item as WeaponInstance;
				break;
			default:
				return;
		}
	}
}