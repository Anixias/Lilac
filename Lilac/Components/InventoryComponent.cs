using System.Collections.Generic;
using Lilac.Items;

namespace Lilac.Components;

public sealed class InventoryComponent : IComponent
{
	private readonly EquipmentComponent? equipmentComponent;
	private readonly List<ItemInstance> items = new();

	public InventoryComponent(EquipmentComponent? equipmentComponent = null)
	{
		this.equipmentComponent = equipmentComponent;
	}

	public IEnumerable<ItemInstance> Items => items;

	public void AddItem(ItemInstance item)
	{
		items.Add(item);
	}

	public void RemoveItem(ItemInstance item)
	{
		items.Remove(item);
		equipmentComponent?.Unequip(item);
	}
}