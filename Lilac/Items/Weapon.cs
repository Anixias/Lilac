namespace Lilac.Items;

public abstract class Weapon : IItem
{
	public abstract Material Material { get; }
	public abstract string Name { get; }
	public abstract string Description { get; }
}