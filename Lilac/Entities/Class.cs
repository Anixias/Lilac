namespace Lilac.Entities;

public sealed class Class
{
	public static readonly Class Warrior = new("Warrior")
	{
		Description = "A mighty agent of war, capable of withstanding heavy blows and dishing out massive damage.",
		Bonuses = new Bonuses
		{
			Strength = +2,
			Constitution = +2,
			Initiative = -2,
			Defense = +2
		}
	};

	public static readonly Class Archer = new("Archer")
	{
		Description = "A swift ranger, delivering precise strikes to targets from afar.",
		Bonuses = new Bonuses
		{
			Constitution = -2,
			Agility = +2,
			Perception = +2,
			Initiative = +2
		}
	};

	public static readonly Class Mage = new("Mage")
	{
		Description = "An adept magi, commanding the desires of magic all around.",
		Bonuses = new Bonuses
		{
			Strength = -2,
			Intelligence = +2,
			Charisma = +2,
			Mana = +5
		}
	};

	private Class(string displayName)
	{
		DisplayName = displayName;
	}

	public string DisplayName { get; }
	public string Description { get; private init; } = "";
	public Bonuses? Bonuses { get; private init; }
}