using System;
using Lilac.Rendering;

namespace Lilac.Menus;

public sealed class Option
{
	public Option(string label)
	: this(label, Array.Empty<string>())
	{
	}

	public Option(string label, params string[] values)
	{
		Label = label;
		Values = values;
	}

	public string[] Values { get; }
	public int SelectedValue { get; set; }
	public string Label { get; }

	public Action? selected;
	public Action<int>? valueChanged;

	public void Display(bool hovered)
	{
		Screen.ForegroundColor = hovered ? StandardColor.Green : StandardColor.White;
		
		Screen.Write($"{(hovered ? "> " : "  ")}");
		Screen.Write(Label);
		
		if (Values.Length > 0)
		{
			var prevArrow = SelectedValue > 0 ? "<- " : "   ";
			var nextArrow = SelectedValue < Values.Length - 1 ? " ->" : "";
			Screen.Write($": {prevArrow}{Values[SelectedValue]}{nextArrow}");
		}

		Screen.WriteLine();
		Screen.ResetColor();
	}

	public void SelectPrevious()
	{
		if (SelectedValue <= 0)
			return;
		
		SelectedValue--;
		valueChanged?.Invoke(SelectedValue);
	}

	public void SelectNext()
	{
		if (SelectedValue >= Values.Length - 1)
			return;
		
		SelectedValue++;
		valueChanged?.Invoke(SelectedValue);
	}

	public void SelectValue(int value)
	{
		SelectedValue = Math.Clamp(value, 0, Values.Length - 1);
	}

	public void Select()
	{
		selected?.Invoke();
	}
}