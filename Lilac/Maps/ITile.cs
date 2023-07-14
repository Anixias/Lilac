using System.Collections.Generic;

namespace Lilac.Maps;

public interface ITile
{
	IMap Map { get; }
	string Name { get; }
	string Description { get; }
	IReadOnlyDictionary<string, ITile> Connections { get; }
	void AddConnection(string command, ITile tile);
	void RemoveConnection(string command);
}