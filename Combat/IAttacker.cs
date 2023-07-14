namespace Lilac.Combat;

public interface IAttacker
{
	bool Attack(IHittable target);
}