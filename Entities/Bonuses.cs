using System;

namespace Lilac.Entities;

public sealed class Bonuses
{
	public int Strength { get; init; }
	public int Agility { get; init; }
	public int Intelligence { get; init; }
	public int Constitution { get; init; }
	public int Perception { get; init; }
	public int Charisma { get; init; }

	public int Health { get; init; }
	public int Mana { get; init; }
	public int Initiative { get; init; }
	public int Defense { get; init; }

	private static string DisplayBonus(int value)
	{
		if (value <= 0)
			return value.ToString();

		return "+" + value;
	}

	public void Display()
	{
		var positiveColor = ConsoleColor.DarkGreen;
		var negativeColor = ConsoleColor.DarkRed;
		
		if (Strength != 0)
		{
			Console.Write("Strength: ");
			Console.ForegroundColor = Strength > 0 ? positiveColor : negativeColor;
			Console.WriteLine(DisplayBonus(Strength));
			Console.ResetColor();
		}

		if (Agility != 0)
		{
			Console.Write("Agility: ");
			Console.ForegroundColor = Agility > 0 ? positiveColor : negativeColor;
			Console.WriteLine(DisplayBonus(Agility));
			Console.ResetColor();
		}

		if (Intelligence != 0)
		{
			Console.Write("Intelligence: ");
			Console.ForegroundColor = Intelligence > 0 ? positiveColor : negativeColor;
			Console.WriteLine(DisplayBonus(Intelligence));
			Console.ResetColor();
		}

		if (Constitution != 0)
		{
			Console.Write("Constitution: ");
			Console.ForegroundColor = Constitution > 0 ? positiveColor : negativeColor;
			Console.WriteLine(DisplayBonus(Constitution));
			Console.ResetColor();
		}

		if (Perception != 0)
		{
			Console.Write("Perception: ");
			Console.ForegroundColor = Perception > 0 ? positiveColor : negativeColor;
			Console.WriteLine(DisplayBonus(Perception));
			Console.ResetColor();
		}

		if (Charisma != 0)
		{
			Console.Write("Charisma: ");
			Console.ForegroundColor = Charisma > 0 ? positiveColor : negativeColor;
			Console.WriteLine(DisplayBonus(Charisma));
			Console.ResetColor();
		}

		if (Health != 0)
		{
			Console.Write("Health: ");
			Console.ForegroundColor = Health > 0 ? positiveColor : negativeColor;
			Console.WriteLine(DisplayBonus(Health));
			Console.ResetColor();
		}

		if (Mana != 0)
		{
			Console.Write("Mana: ");
			Console.ForegroundColor = Mana > 0 ? positiveColor : negativeColor;
			Console.WriteLine(DisplayBonus(Mana));
			Console.ResetColor();
		}

		if (Initiative != 0)
		{
			Console.Write("Initiative: ");
			Console.ForegroundColor = Initiative > 0 ? positiveColor : negativeColor;
			Console.WriteLine(DisplayBonus(Initiative));
			Console.ResetColor();
		}

		if (Defense != 0)
		{
			Console.Write("Defense: ");
			Console.ForegroundColor = Defense > 0 ? positiveColor : negativeColor;
			Console.WriteLine(DisplayBonus(Defense));
			Console.ResetColor();
		}

		Console.WriteLine();
	}
}