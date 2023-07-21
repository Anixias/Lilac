namespace Lilac.Items;

public sealed class Amulet : IItem
{
	public Amulet(string name, string description)
	{
		Name = name;
		Description = description;
	}

	public string Name { get; }
	public string Description { get; }
	public EquipmentSlot EquipmentSlot => EquipmentSlot.Amulet;
}