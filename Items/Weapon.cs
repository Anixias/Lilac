namespace Lilac.Items;

public abstract class Weapon : IItem
{
	public abstract string Name { get; }
	public abstract string Description { get; }
	public abstract Material Material { get; }
}