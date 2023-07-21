using System.Collections.Generic;
using Lilac.Combat;

namespace Lilac.Items;

public abstract class Armor : IItem
{
	private Armor(string name, string description, Dictionary<DamageCategory, int> defenses, int stealthAdvantage,
				  int initiativeBonus)
	{
		Name = name;
		Description = description;
		Defenses = defenses;
		StealthAdvantage = stealthAdvantage;
		InitiativeBonus = initiativeBonus;
	}

	public IReadOnlyDictionary<DamageCategory, int> Defenses { get; }
	public int StealthAdvantage { get; }
	public int InitiativeBonus { get; }
	public string Name { get; }
	public string Description { get; }
	public EquipmentSlot EquipmentSlot => EquipmentSlot.Armor;

	public sealed class MetalArmor : Armor
	{
		public MetalArmor(string name, string description, Dictionary<DamageCategory, int> defenses,
						  int stealthAdvantage, int initiativeBonus) : base(name, description, defenses,
			stealthAdvantage, initiativeBonus)
		{
		}

		public ArmorInstance Instantiate(Material.Metal material)
		{
			return new ArmorInstance(this, material);
		}
	}

	public sealed class LeatherArmor : Armor
	{
		public LeatherArmor(string name, string description, Dictionary<DamageCategory, int> defenses,
							int stealthAdvantage, int initiativeBonus) : base(name, description, defenses,
			stealthAdvantage, initiativeBonus)
		{
		}

		public ArmorInstance Instantiate(Material.Leather material)
		{
			return new ArmorInstance(this, material);
		}
	}

	#region Light Armor

	public static readonly LeatherArmor Tunic = new("Tunic", "A light leather tunic.",
		new Dictionary<DamageCategory, int>
		{
			{ DamageCategory.Physical, 1 },
			{ DamageCategory.Magical, 1 }
		}, 0, 0);

	public static readonly LeatherArmor Robes = new("Robes", "A light set of magic-resistant robes.",
		new Dictionary<DamageCategory, int>
		{
			{ DamageCategory.Magical, 2 }
		}, 0, 0);

	public static readonly LeatherArmor Hide = new("Hide", "A light physical-resistant hide.",
		new Dictionary<DamageCategory, int>
		{
			{ DamageCategory.Physical, 2 }
		}, 0, 0);

	#endregion

	#region Medium Armor

	public static readonly MetalArmor Chainmail = new("Chainmail", "A medium set of chainmail.",
		new Dictionary<DamageCategory, int>
		{
			{ DamageCategory.Physical, 2 },
			{ DamageCategory.Magical, 2 }
		}, -1, -2);

	public static readonly MetalArmor Scale = new("Scale", "A medium set of scale mail.",
		new Dictionary<DamageCategory, int>
		{
			{ DamageCategory.Physical, 3 },
			{ DamageCategory.Magical, 1 }
		}, -1, -2);

	public static readonly MetalArmor Breastplate = new("Breastplate", "A medium breastplate.",
		new Dictionary<DamageCategory, int>
		{
			{ DamageCategory.Physical, 4 }
		}, -1, -2);

	#endregion

	#region Heavy Armor

	public static readonly MetalArmor HalfPlate = new("Half-Plate", "A heavy set of half-plate.",
		new Dictionary<DamageCategory, int>
		{
			{ DamageCategory.Physical, 4 },
			{ DamageCategory.Magical, 2 }
		}, -2, -4);

	public static readonly MetalArmor FullPlate = new("Full-Plate", "A heavy set of full-plate.",
		new Dictionary<DamageCategory, int>
		{
			{ DamageCategory.Physical, 5 },
			{ DamageCategory.Magical, 1 }
		}, -2, -4);

	public static readonly MetalArmor Spellguard = new("Spellguard", "A heavy magic-resistant armor set.",
		new Dictionary<DamageCategory, int>
		{
			{ DamageCategory.Physical, 3 },
			{ DamageCategory.Magical, 3 }
		}, -2, -4);

	#endregion
}