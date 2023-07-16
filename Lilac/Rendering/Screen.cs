using System;

namespace Lilac.Rendering;

public static class Screen
{
	private static bool mainBuffer = true;
	private static IColor foregroundColor = StandardColor.Gray;
	private static IColor backgroundColor = FlashingColor.Reset;
	private static bool cursorVisible = true;
	private static bool strikethrough;
	private static bool italics;
	private static bool bold;
	private static bool underline;

	public static bool CursorVisible
	{
		get => cursorVisible;
		set
		{
			cursorVisible = value;
			Console.Write(cursorVisible ? "\x1b[?25h" : "\x1b[?25l");
		}
	}

	public static bool Strikethrough
	{
		get => strikethrough;
		set
		{
			strikethrough = value;
			Console.Write(strikethrough ? "\x1b[9m" : "\x1b[29m");
		}
	}

	public static bool Italics
	{
		get => italics;
		set
		{
			italics = value;
			Console.Write(italics ? "\x1b[3m" : "\x1b[23m");
		}
	}

	public static bool Bold
	{
		get => bold;
		set
		{
			bold = value;
			Console.Write(bold ? "\x1b[1m" : "\x1b[22m");
		}
	}

	public static bool Underline
	{
		get => underline;
		set
		{
			underline = value;
			Console.Write(underline ? "\x1b[4m" : "\x1b[24m");
		}
	}

	public static void ResetStyles()
	{
		Console.Write("\x1b[0m");
	}

	public static string? ReadLine() => Console.ReadLine();
	public static ConsoleKeyInfo ReadKey() => Console.ReadKey();
	public static int Read() => Console.Read();

	public static void SwapActiveBuffer()
	{
		mainBuffer = !mainBuffer;
		Console.Write(mainBuffer ? "\x1b[?1049l" : "\x1b[?1049h");
	}

	public static void ResetColor()
	{
		foregroundColor = StandardColor.Gray;
		backgroundColor = FlashingColor.Reset;
		Console.Write("\x1b[39;49m");
	}

	public static void Write(object? value)
	{
		Console.Write(value);
	}
	
	public static void WriteLine(object? value)
	{
		Console.WriteLine(value);
	}

	public static void WriteLine()
	{
		Console.WriteLine();
	}

	public static void Clear()
	{
		Console.Clear();
	}

	public static IColor ForegroundColor
	{
		get => foregroundColor;
		set
		{
			foregroundColor = value;
			Console.Write($"\x1b[{foregroundColor.PrintForeground()}m");
		}
	}

	public static IColor BackgroundColor
	{
		get => backgroundColor;
		set
		{
			backgroundColor = value;
			Console.Write($"\x1b[{backgroundColor.PrintBackground()}m");
		}
	}
}