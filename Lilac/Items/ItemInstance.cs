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