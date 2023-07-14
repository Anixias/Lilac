using System;

namespace Lilac.Entities;

public sealed class Character
{
    public string Name { get; set; } = "Player";
    public ConsoleColor Color { get; set; } = ConsoleColor.Red;
    public Class Class { get; set; } = Class.Warrior;
    public Race Race { get; set; } = Race.Human;

	public void Display()
	{
		var prevColor = Console.ForegroundColor;
		Console.ForegroundColor = Color;
		Console.Write(Name);
		Console.ForegroundColor = prevColor;
	}
}