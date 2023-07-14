using System;

namespace Lilac.Menus;

public sealed class MainMenu : Menu
{
	public event EventHandler? OnPlaySelected;
	public event EventHandler? OnSettingsSelected;
	public event EventHandler? OnQuitSelected;

	public MainMenu()
	{
		Options = new[]
		{
			new Option("Play")
			{
				selected = () => { OnPlaySelected?.Invoke(); }
			},
			new Option("Settings")
			{
				selected = () => { OnSettingsSelected?.Invoke(); }
			},
			new Option("Quit")
			{
				selected = () => { OnQuitSelected?.Invoke(); }
			}
		};
	}

	public override void RenderTitle()
	{
		Console.ForegroundColor = ConsoleColor.Magenta;

		Console.WriteLine("#################################");
		Console.Write("# =========== ");
		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.Write("LILAC");
		Console.ForegroundColor = ConsoleColor.Magenta;
		Console.WriteLine(" =========== #");
		Console.WriteLine("#################################");

		Console.ResetColor();
	}
}