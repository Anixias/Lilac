namespace Lilac.Rendering;

public interface IColor
{
	string PrintForeground();
	string PrintBackground();
}

public readonly struct FlashingColor : IColor
{
	private byte ForegroundID { get; }
	private byte BackgroundID { get; }

	public FlashingColor(byte foregroundID, byte backgroundID)
	{
		ForegroundID = foregroundID;
		BackgroundID = backgroundID;
	}
	
	public string PrintForeground()
	{
		return $"5;{ForegroundID}";
	}
	
	public string PrintBackground()
	{
		return $"5;{BackgroundID}";
	}

	public static readonly FlashingColor Black = new(30, 40);
	public static readonly FlashingColor DarkRed = new(31, 41);
	public static readonly FlashingColor DarkGreen = new(32, 42);
	public static readonly FlashingColor DarkYellow = new(33, 43);
	public static readonly FlashingColor DarkBlue = new(34, 44);
	public static readonly FlashingColor DarkMagenta = new(35, 45);
	public static readonly FlashingColor DarkCyan = new(36, 46);
	public static readonly FlashingColor Gray = new(37, 47);
	public static readonly FlashingColor Default = new(39, 49);
	
	public static readonly FlashingColor DarkGray = new(90, 100);
	public static readonly FlashingColor Red = new(91, 101);
	public static readonly FlashingColor Green = new(92, 102);
	public static readonly FlashingColor Yellow = new(93, 103);
	public static readonly FlashingColor Blue = new(94, 104);
	public static readonly FlashingColor Magenta = new(95, 105);
	public static readonly FlashingColor Cyan = new(96, 106);
	public static readonly FlashingColor White = new(97, 107);
	
	public static readonly FlashingColor Reset = new(0, 0);
}

public readonly struct StandardColor : IColor
{
	private byte ID { get; }

	public StandardColor(byte id)
	{
		ID = id;
	}
	
	public string PrintForeground()
	{
		return $"38;5;{ID}";
	}
	
	public string PrintBackground()
	{
		return $"48;5;{ID}";
	}

	public static readonly StandardColor Black = new(0);
	public static readonly StandardColor DarkRed = new(1);
	public static readonly StandardColor DarkGreen = new(2);
	public static readonly StandardColor DarkYellow = new(3);
	public static readonly StandardColor DarkBlue = new(4);
	public static readonly StandardColor DarkMagenta = new(5);
	public static readonly StandardColor DarkCyan = new(6);
	public static readonly StandardColor Gray = new(7);
	
	public static readonly StandardColor DarkGray = new(8);
	public static readonly StandardColor Red = new(9);
	public static readonly StandardColor Green = new(10);
	public static readonly StandardColor Yellow = new(11);
	public static readonly StandardColor Blue = new(12);
	public static readonly StandardColor Magenta = new(13);
	public static readonly StandardColor Cyan = new(14);
	public static readonly StandardColor White = new(15);
}

public struct Color : IColor
{
	public byte Red { get; set; }
	public byte Green { get; set; }
	public byte Blue { get; set; }

	public Color(byte red, byte green, byte blue)
	{
		Red = red;
		Green = green;
		Blue = blue;
	}

	public string PrintForeground()
	{
		return $"38;2;{Red};{Green};{Blue}";
	}

	public string PrintBackground()
	{
		return $"48;2;{Red};{Green};{Blue}";
	}
}