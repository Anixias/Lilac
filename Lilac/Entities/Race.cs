using Lilac.Dice;

namespace Lilac.Entities;

public sealed class Race
{
	public static readonly Race Human = new("Human")
	{
		Description =
			"Humans are the most common race in Eldora. They boast a balanced distribution of abilities and attributes. Humans provide many innovations and are very strategic.",
		Bonuses = new Bonuses
		{
			Intelligence = +2
		},
		AttributeRolls = new AttributeRolls
		{
			Strength = new Roll.Binary.Addition(new Roll.Modifier(6), Roll.Die.D6),
			Agility = new Roll.Binary.Addition(new Roll.Modifier(6), Roll.Die.D6),
			Intelligence = new Roll.Binary.Addition(new Roll.Modifier(6), Roll.Die.D6),
			Constitution = new Roll.Binary.Addition(new Roll.Modifier(6), Roll.Die.D6),
			Perception = new Roll.Binary.Addition(new Roll.Modifier(6), Roll.Die.D6),
			Charisma = new Roll.Binary.Addition(new Roll.Modifier(6), Roll.Die.D6)
		}
	};

	public static readonly Race Elf = new("Elf")
	{
		Description =
			"Elvenkind are long-lived and magically inclined, known for their wisdom and connection with nature.",
		Bonuses = new Bonuses
		{
			Agility = +2
		},
		AttributeRolls = new AttributeRolls
		{
			Strength = new Roll.Binary.Addition(new Roll.Modifier(5), Roll.Die.D6),
			Agility = new Roll.Binary.Addition(new Roll.Modifier(7), Roll.Die.D6),
			Intelligence = new Roll.Binary.Addition(new Roll.Modifier(7), Roll.Die.D6),
			Constitution = new Roll.Binary.Addition(new Roll.Modifier(5), Roll.Die.D6),
			Perception = new Roll.Binary.Addition(new Roll.Modifier(6), Roll.Die.D6),
			Charisma = new Roll.Binary.Addition(new Roll.Modifier(6), Roll.Die.D6)
		}
	};

	public static readonly Race Dwarf = new("Dwarf")
	{
		Description = "Dwarves are sturdy and resilient, renowned for their craftsmanship and martial prowess.",
		Bonuses = new Bonuses
		{
			Constitution = +2
		},
		AttributeRolls = new AttributeRolls
		{
			Strength = new Roll.Binary.Addition(new Roll.Modifier(7), Roll.Die.D6),
			Agility = new Roll.Binary.Addition(new Roll.Modifier(5), Roll.Die.D6),
			Intelligence = new Roll.Binary.Addition(new Roll.Modifier(6), Roll.Die.D6),
			Constitution = new Roll.Binary.Addition(new Roll.Modifier(7), Roll.Die.D6),
			Perception = new Roll.Binary.Addition(new Roll.Modifier(6), Roll.Die.D6),
			Charisma = new Roll.Binary.Addition(new Roll.Modifier(5), Roll.Die.D6)
		}
	};

	public static readonly Race Orc = new("Orc")
	{
		Description =
			"Typically larger and bulkier than humans, orcs combine physical might with a fierce temper. Often living in tribal societies, they value strength and combat prowess above all. Their rough lifestyle and hardy physique make them exceptional warriors, but they can also be surprisingly cunning and adaptable.",
		Bonuses = new Bonuses
		{
			Strength = +2
		},
		AttributeRolls = new AttributeRolls
		{
			Strength = new Roll.Binary.Addition(new Roll.Modifier(8), Roll.Die.D6),
			Agility = new Roll.Binary.Addition(new Roll.Modifier(5), Roll.Die.D6),
			Intelligence = new Roll.Binary.Addition(new Roll.Modifier(5), Roll.Die.D6),
			Constitution = new Roll.Binary.Addition(new Roll.Modifier(7), Roll.Die.D6),
			Perception = new Roll.Binary.Addition(new Roll.Modifier(6), Roll.Die.D6),
			Charisma = new Roll.Binary.Addition(new Roll.Modifier(5), Roll.Die.D6)
		}
	};

	public static readonly Race Halfling = new("Halfling")
	{
		Description = "Halflings are small and agile, known for their luck and their ability to go unnoticed.",
		Bonuses = new Bonuses
		{
			Perception = +2
		},
		AttributeRolls = new AttributeRolls
		{
			Strength = new Roll.Binary.Addition(new Roll.Modifier(5), Roll.Die.D6),
			Agility = new Roll.Binary.Addition(new Roll.Modifier(7), Roll.Die.D6),
			Intelligence = new Roll.Binary.Addition(new Roll.Modifier(6), Roll.Die.D6),
			Constitution = new Roll.Binary.Addition(new Roll.Modifier(5), Roll.Die.D6),
			Perception = new Roll.Binary.Addition(new Roll.Modifier(7), Roll.Die.D6),
			Charisma = new Roll.Binary.Addition(new Roll.Modifier(6), Roll.Die.D6)
		}
	};

	private Race(string displayName)
	{
		DisplayName = displayName;
	}

	public string DisplayName { get; }
	public string Description { get; private init; } = "";
	public Bonuses? Bonuses { get; private init; }
	public AttributeRolls? AttributeRolls { get; private init; }
}