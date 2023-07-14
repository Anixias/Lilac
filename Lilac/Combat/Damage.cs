namespace Lilac.Combat;

public readonly struct DamageSource
{
	public DamageType Type { get; init; }
	public int Damage { get; init; }
}