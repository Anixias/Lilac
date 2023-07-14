using System.Collections.Generic;

namespace Lilac.Combat;

public abstract class DamageType
{	
	private static readonly List<DamageType> AllDamageTypes = new();
	private static readonly List<Physical> AllPhysicalTypes = new();
	private static readonly List<Magical> AllMagicalTypes = new();
	private static readonly List<Direct> AllDirectTypes = new();
	
	private DamageType(string displayName)
	{
		DisplayName = displayName;
		AllDamageTypes.Add(this);
	}

	public static IEnumerable<DamageType> AllTypes => AllDamageTypes;
	public static IEnumerable<Physical> PhysicalTypes => AllPhysicalTypes;
	public static IEnumerable<Magical> MagicalTypes => AllMagicalTypes;
	public static IEnumerable<Direct> DirectTypes => AllDirectTypes;
	
	public string DisplayName { get; }
	public abstract DamageCategory Category { get; }

	public sealed class Physical : DamageType
	{
		public Physical(string displayName)
		: base(displayName)
		{
			AllPhysicalTypes.Add(this);
		}
		
		public override DamageCategory Category => DamageCategory.Physical;
	}

	public sealed class Magical : DamageType
	{
		public Magical(string displayName)
		: base(displayName)
		{
			AllMagicalTypes.Add(this);
		}
		
		public override DamageCategory Category => DamageCategory.Magical;
	}

	public sealed class Direct : DamageType
	{
		public Direct(string displayName)
		: base(displayName)
		{
			AllDirectTypes.Add(this);
		}
		
		public override DamageCategory Category => DamageCategory.Direct;
	}

	public DamageSource CreateDamage(int damage)
	{
		return new DamageSource
		{
			Damage = damage,
			Type = this
		};
	}

	public static readonly Direct Raw = new("Raw");
	
	public static readonly Physical Slashing = new("Slashing");
	public static readonly Physical Piercing = new("Piercing");
	public static readonly Physical Crushing = new("Crushing");
	
	public static readonly Magical Fire = new("Fire");
	public static readonly Magical Electricity = new("Electricity");
	public static readonly Magical Water = new("Water");
	public static readonly Magical Earth = new("Earth");
	public static readonly Magical Air = new("Air");
	public static readonly Magical Shadow = new("Shadow");
	public static readonly Magical Nature = new("Nature");
	public static readonly Magical Plasma = new("Plasma");
	public static readonly Magical Void = new("Void");
	public static readonly Magical Ice = new("Ice");
	public static readonly Magical Incendiary = new("Incendiary");
	public static readonly Magical Chaos = new("Chaos");
}

public enum DamageCategory
{
	Physical,
	Magical,
	Direct
}