using System;

namespace Lilac.Maps;

public interface IMap
{
	string Name { get; }
	ConsoleColor PrimaryColor { get; }
	ConsoleColor SecondaryColor { get; }
}