using Lilac.Combat;
using Lilac.Dice;

namespace Lilac.Items;

public sealed class Weapon : IItem
{
	public static readonly Weapon Saber =
		new("Saber", "A long, straight sword.", Roll.Die.D8, Attribute.Strength, DamageType.Slashing)
		{
			HitBonus = 0,
			TwoHanded = false
		};

	public static readonly Weapon Scimitar =
		new("Scimitar", "A lightweight curved sword.", Roll.Die.D6, Attribute.Agility,
			DamageType.Slashing)
		{
			HitBonus = +2,
			TwoHanded = false
		};

	public static readonly Weapon Claymore =
		new("Claymore", "A huge, heavy sword.", Roll.Die.D12, Attribute.Strength, DamageType.Slashing)
		{
			HitBonus = -1,
			TwoHanded = true
		};

	private Weapon(string name, string description, Roll damageRoll, Attribute attackAttribute,
				   DamageType damageType)
	{
		Name = name;
		Description = description;
		DamageRoll = damageRoll;
		AttackAttribute = attackAttribute;
		DamageType = damageType;
	}

	public bool TwoHanded { get; init; }
	public int HitBonus { get; init; }
	public Roll DamageRoll { get; }
	public Attribute AttackAttribute { get; }
	public DamageType DamageType { get; }
	public string Name { get; }
	public string Description { get; }
}