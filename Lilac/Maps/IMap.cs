using System;
using Lilac.Rendering;

namespace Lilac.Maps;

public interface IMap
{
	string Name { get; }
	IColor PrimaryColor { get; }
	IColor SecondaryColor { get; }
}