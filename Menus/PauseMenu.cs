using System;

namespace Lilac.Menus;

public sealed class PauseMenu : Menu
{
	public event EventHandler? OnResumeSelected;
	public event EventHandler? OnQuitToMainMenuSelected;
	public event EventHandler? OnQuitToDesktopSelected;
	
	public PauseMenu()
	{
		Options = new[]
		{
			new Option("Resume")
			{
				selected = () => OnResumeSelected?.Invoke()
			},
			new Option("Quit to Main Menu")
			{
				selected = () => OnQuitToMainMenuSelected?.Invoke()
			},
			new Option("Quit to Desktop")
			{
				selected = () => OnQuitToDesktopSelected?.Invoke()
			}
		};
	}
	
    public override void RenderTitle()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;

		Console.Write("# =========== ");
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.Write("Paused");
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" =========== #");

		Console.ResetColor();
    }
}