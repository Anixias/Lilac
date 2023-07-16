using System;
using Lilac.Rendering;

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
        Screen.ForegroundColor = StandardColor.DarkGray;

		Screen.Write("# =========== ");
		Screen.ForegroundColor = StandardColor.Yellow;
		Screen.Write("Paused");
		Screen.ForegroundColor = StandardColor.DarkGray;
		Screen.WriteLine(" =========== #");

		Screen.ResetColor();
    }
}