using System;

namespace Lilac.Menus;

public sealed class SettingsMenu : Menu
{
	public event EventHandler? OnBackSelected;

	public delegate void DifficultyChangedEventHandler(Game.Difficulty difficulty);
	public event DifficultyChangedEventHandler? OnDifficultyChanged;

	public SettingsMenu()
	{
		Options = new[]
		{
			new Option("Difficulty", "Easy", "Normal", "Hard")
			{
				valueChanged = index =>
				{
					OnDifficultyChanged?.Invoke((Game.Difficulty)index);
				}
			},
			new Option("Back")
			{
				selected = () =>
				{
					OnBackSelected?.Invoke();
				}
			}
		};
	}

	public override void RenderTitle()
	{
		Console.ForegroundColor = ConsoleColor.Blue;

		Console.Write("# =========== ");
		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.Write("Settings");
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.WriteLine(" =========== #");

		Console.ResetColor();
	}

	public void SetDifficulty(Game.Difficulty difficulty)
	{
		Options[0].SelectValue((int)difficulty);
	}
}