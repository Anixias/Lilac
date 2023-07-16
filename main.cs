using System;
using Lilac;

internal static class Program
{
	public static void Main(string[] args)
	{
		// Setup The Console
		Console.Write("\x1b[?1049h");        // switch terminal buffer
		Console.CursorVisible = false;
		Console.Clear();
		
		try
		{
			var game = new Game();
			game.Start();

			Console.Clear();
		}
		finally
		{
			Console.CursorVisible = true;
			Console.Write("\x1b[?1049l");    // switch back from terminal buffer
		}
	}
}