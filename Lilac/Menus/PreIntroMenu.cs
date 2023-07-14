using System;

namespace Lilac.Menus;

public sealed class PreIntroMenu : Menu
{
	public event EventHandler? OnOkaySelected;
	
	public PreIntroMenu()
	{
		Options = new[]
		{
			new Option("Okay")
			{
				selected = () => OnOkaySelected?.Invoke()
			}
		};
	}
	
    public override void RenderTitle()
    {
        Console.ForegroundColor = ConsoleColor.Blue;

		Console.Write("# =========== ");
		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.Write("Welcome");
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.WriteLine(" =========== #");

		Console.ResetColor();

		Console.WriteLine("You may press ESC at any time to view the pause menu.");
    }
}