namespace Lilac.Combat;

public interface IHittable
{
	string Name { get; }
	void ReceiveDamage(DamageSource source);
	bool CheckHit(int attackValue);
}