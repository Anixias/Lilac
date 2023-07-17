using System.Collections.Generic;

namespace Lilac.Maps;

public sealed class World
{
	public List<IMap> Maps { get; } = new();

	public void Generate()
	{
	}
}