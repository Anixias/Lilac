namespace Lilac.Items;

public sealed class Shield : IItem
{
	public static readonly Shield Round = new("Round Shield", "A lightweight round shield.", 1, 0);
	public static readonly Shield Square = new("Square Shield", "A medium square shield.", 2, -1);
	public static readonly Shield Kite = new("Kite Shield", "A heavy kite shield.", 3, -2);

	private Shield(string name, string description, int defense, int initiativeBonus)
	{
		Name = name;
		Description = description;
		Defense = defense;
		InitiativeBonus = initiativeBonus;
	}

	public int Defense { get; }
	public int InitiativeBonus { get; }
	public string Name { get; }
	public string Description { get; }
	public EquipmentSlot EquipmentSlot => EquipmentSlot.Shield;

	public ShieldInstance Instantiate(Material material)
	{
		return new ShieldInstance(this, material);
	}
}