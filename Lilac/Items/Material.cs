namespace Lilac.Items;

public abstract class Material
{
	public static readonly Metal Bronze = new("Bronze", 1, 0);
	public static readonly Metal Iron = new("Iron", 3, 1);
	public static readonly Metal Steel = new("Steel", 9, 2);

	private Material(string name, int costMultiplier, int power)
	{
		Name = name;
		CostMultiplier = costMultiplier;
		Power = power;
	}

	public string Name { get; }
	public int CostMultiplier { get; }
	public int Power { get; }

	public sealed class Metal : Material
	{
		public Metal(string name, int costMultiplier, int power)
			: base(name, costMultiplier, power)
		{
		}
	}
}