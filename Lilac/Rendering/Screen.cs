using System;

namespace Lilac.Rendering;

public static class Screen
{
	private static bool mainBuffer = true;
	private static IColor foregroundColor = FlashingColor.Default;
	private static IColor backgroundColor = FlashingColor.Default;

	public static bool CursorVisible
	{
		get => Console.CursorVisible;
		set => Console.CursorVisible = value;
	}

	public static string? ReadLine() => Console.ReadLine();
	public static ConsoleKeyInfo ReadKey() => Console.ReadKey();
	public static int Read() => Console.Read();

	public static void SwapActiveBuffer()
	{
		mainBuffer = !mainBuffer;
		Console.Write(mainBuffer ? "\x1b[?1049l" : "\x1b[?1049h");
	}

	public static void Write(object? value)
	{
		Console.Write(value);
	}
	
	public static void WriteLine(object? value)
	{
		Console.WriteLine(value);
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