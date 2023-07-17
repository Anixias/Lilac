using System;

namespace Lilac.Components;

public sealed class LevelComponent : IComponent
{
	public delegate void LevelEventHandler(int level);

	private const int XPThreshold = 100;
	private const int XPThresholdGrowthRate = 100;
	private int xp;

	public LevelComponent()
	{
		Level = GetLevelFromXP(xp);
	}

	public int Level { get; private set; }

	public int XP
	{
		get => xp;
		set
		{
			xp = Math.Max(0, value);

			var currentLevel = Level;
			Level = GetLevelFromXP(xp);

			if (Level != currentLevel)
				OnLevelChanged?.Invoke(Level);
		}
	}

	public event LevelEventHandler? OnLevelChanged;

	public static int GetLevelFromXP(int xp)
	{
		var level = 1;
		var threshold = XPThreshold;

		while (xp >= threshold)
		{
			level++;
			xp -= threshold;
			threshold += XPThresholdGrowthRate;
		}

		return level;
	}

	public static int GetXPFromLevel(int level)
	{
		if (level < 1) throw new ArgumentException("Level must be positive");

		if (level == 1) return 0;

		var threshold = XPThreshold;
		var xp = 0;

		for (var i = 1; i < level; i++)
		{
			xp += threshold;
			threshold += XPThresholdGrowthRate;
		}

		return xp;
	}
}