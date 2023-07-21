using Lilac.Combat;
using Lilac.Dice;

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
	public Roll DamageRoll => ((Weapon)Framework).DamageRoll + Material.Power;
	public int HitBonus => ((Weapon)Framework).HitBonus;
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
		var baseArmor = ((Armor)Framework).Defenses.TryGetValue(damageType.Category, out var armor) ? armor : 0;
		return baseArmor + Material.Power;
	}
}

public sealed class AmuletInstance : ItemInstance
{
	public AmuletInstance(Amulet framework)
		: base(framework)
	{
	}
}