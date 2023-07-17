using Lilac.Rendering;

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
		var positiveColor = StandardColor.DarkGreen;
		var negativeColor = StandardColor.DarkRed;

		if (Strength != 0)
		{
			Screen.Write("Strength: ");
			Screen.ForegroundColor = Strength > 0 ? positiveColor : negativeColor;
			Screen.WriteLine(DisplayBonus(Strength));
			Screen.ResetColor();
		}

		if (Agility != 0)
		{
			Screen.Write("Agility: ");
			Screen.ForegroundColor = Agility > 0 ? positiveColor : negativeColor;
			Screen.WriteLine(DisplayBonus(Agility));
			Screen.ResetColor();
		}

		if (Intelligence != 0)
		{
			Screen.Write("Intelligence: ");
			Screen.ForegroundColor = Intelligence > 0 ? positiveColor : negativeColor;
			Screen.WriteLine(DisplayBonus(Intelligence));
			Screen.ResetColor();
		}

		if (Constitution != 0)
		{
			Screen.Write("Constitution: ");
			Screen.ForegroundColor = Constitution > 0 ? positiveColor : negativeColor;
			Screen.WriteLine(DisplayBonus(Constitution));
			Screen.ResetColor();
		}

		if (Perception != 0)
		{
			Screen.Write("Perception: ");
			Screen.ForegroundColor = Perception > 0 ? positiveColor : negativeColor;
			Screen.WriteLine(DisplayBonus(Perception));
			Screen.ResetColor();
		}

		if (Charisma != 0)
		{
			Screen.Write("Charisma: ");
			Screen.ForegroundColor = Charisma > 0 ? positiveColor : negativeColor;
			Screen.WriteLine(DisplayBonus(Charisma));
			Screen.ResetColor();
		}

		if (Health != 0)
		{
			Screen.Write("Health: ");
			Screen.ForegroundColor = Health > 0 ? positiveColor : negativeColor;
			Screen.WriteLine(DisplayBonus(Health));
			Screen.ResetColor();
		}

		if (Mana != 0)
		{
			Screen.Write("Mana: ");
			Screen.ForegroundColor = Mana > 0 ? positiveColor : negativeColor;
			Screen.WriteLine(DisplayBonus(Mana));
			Screen.ResetColor();
		}

		if (Initiative != 0)
		{
			Screen.Write("Initiative: ");
			Screen.ForegroundColor = Initiative > 0 ? positiveColor : negativeColor;
			Screen.WriteLine(DisplayBonus(Initiative));
			Screen.ResetColor();
		}

		if (Defense != 0)
		{
			Screen.Write("Defense: ");
			Screen.ForegroundColor = Defense > 0 ? positiveColor : negativeColor;
			Screen.WriteLine(DisplayBonus(Defense));
			Screen.ResetColor();
		}

		Screen.WriteLine();
	}
}