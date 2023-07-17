using Lilac;
using Lilac.Rendering;

internal static class Program
{
	public static void Main(string[] args)
	{
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
	}
}