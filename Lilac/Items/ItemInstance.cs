using System;
using Lilac.Combat;
using Lilac.Dice;
using Attribute = Lilac.Combat.Attribute;

namespace Lilac.Items;

public abstract class ItemInstance
{
	protected ItemInstance(IItem framework)
	{
		Framework = framework;
	}

	public IItem Framework { get; }
	public virtual string Name => Framework.Name;
}

public sealed class WeaponInstance : ItemInstance
{
	public WeaponInstance(Weapon framework, Material material)
		: base(framework)
	{
		Material = material;
	}

	private Material Material { get; }
	public override string Name => $"{Material.Name} {Framework.Name}";

	public Roll DamageRoll =>
		((Weapon)Framework).DamageRoll + (Material.Power > 0 ? (int)Math.Pow(2, Material.Power) : 0);

	public int HitBonus => ((Weapon)Framework).HitBonus + Material.Power;
	public Attribute AttackAttribute => ((Weapon)Framework).AttackAttribute;
	public DamageType DamageType => ((Weapon)Framework).DamageType;
	public bool TwoHanded => ((Weapon)Framework).TwoHanded;
}

public sealed class ArmorInstance : ItemInstance
{
	public ArmorInstance(Armor framework, Material material)
		: base(framework)
	{
		Material = material;
	}

	private Material Material { get; }
	public override string Name => $"{Material.Name} {Framework.Name}";
	public int StealthAdvantage => ((Armor)Framework).StealthAdvantage;
	public int InitiativeBonus => ((Armor)Framework).InitiativeBonus;

	public int GetArmor(DamageType damageType)
	{
		return GetArmor(damageType.Category);
	}

	public int GetArmor(DamageCategory damageCategory)
	{
		var baseArmor = ((Armor)Framework).Defenses.TryGetValue(damageCategory, out var armor) ? armor : 0;
		return baseArmor + Material.Power * 2;
	}
}

public sealed class ShieldInstance : ItemInstance
{
	public ShieldInstance(Shield framework, Material material)
		: base(framework)
	{
		Material = material;
	}

	private Material Material { get; }
	public override string Name => $"{Material.Name} {Framework.Name}";
	public int InitiativeBonus => ((Shield)Framework).InitiativeBonus;
	public int Defense => ((Shield)Framework).Defense + Material.Power;
}

public sealed class AmuletInstance : ItemInstance
{
	public AmuletInstance(Amulet framework)
		: base(framework)
	{
	}
}