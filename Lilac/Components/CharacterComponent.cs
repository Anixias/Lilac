using Lilac.Dice;
using Lilac.Entities;

namespace Lilac.Components;

public sealed class CharacterComponent : IComponent
{
	public CharacterComponent(Character character, StatsComponent statsComponent)
	{
		Character = character;
		StatsComponent = statsComponent;

		if (Character.Class.Bonuses is not null)
			StatsComponent.bonuses.Add(Character.Class.Bonuses);

		if (Character.Race.Bonuses is not null)
			StatsComponent.bonuses.Add(Character.Race.Bonuses);

		GenerateAttributes();
	}

	public Character Character { get; }
	public StatsComponent StatsComponent { get; }

	private void GenerateAttributes()
	{
		var strengthBonus = (Character.Race.Bonuses?.Strength ?? 0) + (Character.Class.Bonuses?.Strength ?? 0);
		var agilityBonus = (Character.Race.Bonuses?.Agility ?? 0) + (Character.Class.Bonuses?.Agility ?? 0);
		var intelligenceBonus =
			(Character.Race.Bonuses?.Intelligence ?? 0) + (Character.Class.Bonuses?.Intelligence ?? 0);
		var constitutionBonus =
			(Character.Race.Bonuses?.Constitution ?? 0) + (Character.Class.Bonuses?.Constitution ?? 0);
		var perceptionBonus = (Character.Race.Bonuses?.Perception ?? 0) + (Character.Class.Bonuses?.Perception ?? 0);
		var charismaBonus = (Character.Race.Bonuses?.Charisma ?? 0) + (Character.Class.Bonuses?.Charisma ?? 0);

		var strengthRoll = (Character.Race.AttributeRolls?.Strength ?? new Roll.Modifier(0)) + strengthBonus;
		var agilityRoll = (Character.Race.AttributeRolls?.Agility ?? new Roll.Modifier(0)) + agilityBonus;
		var intelligenceRoll =
			(Character.Race.AttributeRolls?.Intelligence ?? new Roll.Modifier(0)) + intelligenceBonus;
		var constitutionRoll =
			(Character.Race.AttributeRolls?.Constitution ?? new Roll.Modifier(0)) + constitutionBonus;
		var perceptionRoll = (Character.Race.AttributeRolls?.Perception ?? new Roll.Modifier(0)) + perceptionBonus;
		var charismaRoll = (Character.Race.AttributeRolls?.Charisma ?? new Roll.Modifier(0)) + charismaBonus;

		StatsComponent.Strength = strengthRoll.Execute().Value;
		StatsComponent.Agility = agilityRoll.Execute().Value;
		StatsComponent.Intelligence = intelligenceRoll.Execute().Value;
		StatsComponent.Constitution = constitutionRoll.Execute().Value;
		StatsComponent.Perception = perceptionRoll.Execute().Value;
		StatsComponent.Charisma = charismaRoll.Execute().Value;
	}
}