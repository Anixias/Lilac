using System;

namespace Lilac.Rendering;

public static class Drawing
{
	public static void DrawBar(int width, double percent, IColor color)
	{
		Screen.ResetColor();
		Screen.Write("[");
		var filled = (int)Math.Ceiling(width * percent);
		Screen.BackgroundColor = color;
		if (filled > 0)
			Screen.Write(new string(' ', filled));
		
		Screen.ResetColor();

		if (width - filled > 0)
			Screen.Write(new string(' ', width - filled));
		
		Screen.Write("]");
	}
}