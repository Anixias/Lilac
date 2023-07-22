using System.Collections.Generic;
using Lilac.Combat;
using Lilac.Items;
using Lilac.Rendering;

namespace Lilac.Entities;

public sealed class Character
{
	public string Name { get; set; } = "Player";
	public IColor Color { get; set; } = StandardColor.Red;
	public Class Class { get; set; } = Class.Warrior;
	public Race Race { get; set; } = Race.Human;
	public Dictionary<DamageType.Magical, int> Affinities { get; } = new();
	public Weapon? StartingWeapon { get; set; }
	public Armor? StartingArmor { get; set; }

	public void Display()
	{
		var prevColor = Screen.ForegroundColor;
		Screen.ForegroundColor = Color;
		Screen.Write(Name);
		Screen.ForegroundColor = prevColor;
	}
}