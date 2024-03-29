using System;
using System.Collections.Generic;

namespace Lilac.Menus;

public interface IMenu
{
    void RenderTitle();
    void RenderOptions();
    bool HandleKey(ConsoleKeyInfo key);
}

public abstract class MenuContainer : IMenu
{
    protected IMenu? currentMenu;

	protected readonly Dictionary<ConsoleKeyInfo, Action> customKeyEvents = new();

    protected abstract void RenderContainerTitle();

    public void RenderTitle()
    {
        RenderContainerTitle();
        currentMenu?.RenderTitle();
    }

    public void RenderOptions()
    {
        currentMenu?.RenderOptions();
    }

    public bool HandleKey(ConsoleKeyInfo key)
    {
		if (customKeyEvents.TryGetValue(key, out var customEvent))
		{
			customEvent.Invoke();
			return true;
		}
		
        return currentMenu?.HandleKey(key) ?? false;
    }
}

public abstract class Prompt : IMenu
{
    public delegate void InputEventHandler(string input);
    public event InputEventHandler? OnInputSubmitted;

    protected string Input { get; set; } = "";

    public void SetInput(string input)
    {
        Input = input;
    }

    public abstract void RenderTitle();

    public virtual void RenderOptions()
    {
        Console.WriteLine("> " + Input + "|");
    }

    public virtual bool HandleKey(ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.Escape:
                return false;
            case ConsoleKey.Enter:
                OnInputSubmitted?.Invoke(Input);
                Input = "";
                break;
            case ConsoleKey.Backspace:
                if (Input.Length > 0)
                {
                    Input = Input.Remove(Input.Length - 1);
                }
                break;
            default:
                Input += key.KeyChar;
                break;
        }

        return true;
    }
}

public abstract class Menu : IMenu
{
    public delegate void EventHandler();
    private int hoverIndex;
	private Option[] options;

    protected Menu()
    {
        options = Array.Empty<Option>();
    }

    protected Option[] Options
	{
		get => options;
        set
		{
			options = value;
			hoverIndex = 0;
		}
	}

    public abstract void RenderTitle();

    public virtual void RenderOptions()
    {
        for (var i = 0; i < Options.Length; i++)
        {
            var option = Options[i];
            option.Display(hoverIndex == i);
        }
    }

    public virtual bool HandleKey(ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.Escape:
                return false;
            case ConsoleKey.UpArrow:
                hoverIndex = Math.Max(0, hoverIndex - 1);
                break;
            case ConsoleKey.DownArrow:
                hoverIndex = Math.Min(Options.Length - 1, hoverIndex + 1);
                break;
            case ConsoleKey.LeftArrow:
				if (hoverIndex >= 0 && hoverIndex < Options.Length)
                	Options[hoverIndex].SelectPrevious();
                break;
            case ConsoleKey.RightArrow:
				if (hoverIndex >= 0 && hoverIndex < Options.Length)
                	Options[hoverIndex].SelectNext();
                break;
            case ConsoleKey.Enter:
				if (hoverIndex >= 0 && hoverIndex < Options.Length)
                	Options[hoverIndex].Select();
                break;
        }

        return true;
    }
}