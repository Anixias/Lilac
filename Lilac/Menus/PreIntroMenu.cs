using Lilac.Rendering;

namespace Lilac.Menus;

public sealed class PreIntroMenu : Menu
{
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

	public event EventHandler? OnOkaySelected;

	public override void RenderTitle()
	{
		Screen.ForegroundColor = StandardColor.Blue;

		Screen.Write("# =========== ");
		Screen.ForegroundColor = StandardColor.Cyan;
		Screen.Write("Welcome");
		Screen.ForegroundColor = StandardColor.Blue;
		Screen.WriteLine(" =========== #");

		Screen.ResetColor();

		Screen.WriteLine("You may press ESC at any time to view the pause menu.");
	}
}