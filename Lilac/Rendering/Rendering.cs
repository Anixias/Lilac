using System;

namespace Lilac.Rendering;

public static class Drawing
{
	public static void DrawBar(int width, double percent, ConsoleColor color)
	{
		Console.ResetColor();
		Console.Write("[");
		var filled = (int)Math.Ceiling(width * percent);
		Console.BackgroundColor = color;
		if (filled > 0)
			Console.Write(new string(' ', filled));
		
		Console.ResetColor();

		if (width - filled > 0)
			Console.Write(new string(' ', width - filled));
		
		Console.Write("]");
	}
}