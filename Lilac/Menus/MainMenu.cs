using Lilac.Rendering;

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
		Screen.ForegroundColor = StandardColor.Magenta;

		Screen.WriteLine("#################################");
		Screen.Write("# =========== ");
		Screen.ForegroundColor = StandardColor.Cyan;
		Screen.Write("LILAC");
		Screen.ForegroundColor = StandardColor.Magenta;
		Screen.WriteLine(" =========== #");
		Screen.WriteLine("#################################");

		Screen.ResetColor();
	}
}