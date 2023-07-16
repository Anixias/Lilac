using System;
using Lilac.Rendering;

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
		Screen.ForegroundColor = StandardColor.Blue;

		Screen.Write("# =========== ");
		Screen.ForegroundColor = StandardColor.Cyan;
		Screen.Write("Settings");
		Screen.ForegroundColor = StandardColor.Blue;
		Screen.WriteLine(" =========== #");

		Screen.ResetColor();
	}

	public void SetDifficulty(Game.Difficulty difficulty)
	{
		Options[0].SelectValue((int)difficulty);
	}
}