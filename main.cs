using Lilac;
using Lilac.Rendering;
using Lilac.Generation;

internal static class Program
{
	public static void Main(string[] args)
	{
/*
		// Setup The Console
		Screen.SwapActiveBuffer();
		Screen.CursorVisible = false;
		Screen.Clear();

		try
		{
			var game = new Game();
			game.Start();

			Screen.Clear();
		}
		finally
		{
			Screen.CursorVisible = true;
			Screen.SwapActiveBuffer();
		}
*/
		// var graph = new Graph();
		Graph graph = new Graph(25, 20);
	}
}