namespace Lilac.Items;

public abstract class Material
{
	public static readonly Metal Bronze = new("Bronze", 1);
	public static readonly Metal Iron = new("Iron", 3);
	public static readonly Metal Steel = new("Steel", 9);

	private Material(string name, int costMultiplier)
	{
		Name = name;
		CostMultiplier = costMultiplier;
	}

	public string Name { get; }
	public int CostMultiplier { get; }

	public sealed class Metal : Material
	{
		public Metal(string name, int costMultiplier)
			: base(name, costMultiplier)
		{
		}
	}
}