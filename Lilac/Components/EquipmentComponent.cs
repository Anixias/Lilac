using Lilac.Items;

namespace Lilac.Components;

public sealed class EquipmentComponent : IComponent
{
	public delegate void EventHandler();

	private AmuletInstance? amulet;
	private ArmorInstance? armor;
	private ShieldInstance? shield;
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

	public ArmorInstance? Armor
	{
		get => armor;
		private set
		{
			armor = value;
			OnEquipmentChanged?.Invoke();
		}
	}

	public AmuletInstance? Amulet
	{
		get => amulet;
		private set
		{
			amulet = value;
			OnEquipmentChanged?.Invoke();
		}
	}

	public ShieldInstance? Shield
	{
		get => shield;
		private set
		{
			shield = value;
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

		if (Shield == item)
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

		if (Shield == item)
			Shield = null;
	}

	/// <summary>
	///     Equips the given <see cref="ItemInstance" /> if it can be equipped.
	/// </summary>
	/// <param name="item">The item to equip.</param>
	/// <returns><see langword="true" /> if the item was equipped; <see langword="false" /> otherwise.</returns>
	public bool Equip(ItemInstance item)
	{
		switch (item.Framework.EquipmentSlot)
		{
			case EquipmentSlot.Weapon:
				if (((WeaponInstance)item).TwoHanded && Shield is not null)
					return false;

				Weapon = item as WeaponInstance;
				break;
			case EquipmentSlot.Armor:
				Armor = item as ArmorInstance;
				break;
			case EquipmentSlot.Amulet:
				Amulet = item as AmuletInstance;
				break;
			case EquipmentSlot.Shield:
				if (Weapon?.TwoHanded ?? false)
					return false;

				Shield = item as ShieldInstance;
				break;
			default:
				return false;
		}

		return true;
	}
}