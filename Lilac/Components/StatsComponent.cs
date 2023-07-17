using System;
using System.Collections.Generic;
using Lilac.Entities;

namespace Lilac.Components;

public sealed class StatsComponent : IComponent
{
	public delegate void EventHandler();

	public enum Attribute : byte
	{
		Strength,
		Agility,
		Intelligence,
		Constitution,
		Perception,
		Charisma
	}

	public readonly List<Bonuses> bonuses = new();

	private readonly HealthComponent? healthComponent;
	private readonly LevelComponent? levelComponent;
	private readonly ManaComponent? manaComponent;
	private int agility;
	private int charisma;
	private int constitution;
	private int intelligence;
	private int perception;

	private int strength;

	public StatsComponent(HealthComponent? healthComponent, ManaComponent? manaComponent,
						  LevelComponent? levelComponent)
	{
		this.healthComponent = healthComponent;
		this.manaComponent = manaComponent;
		this.levelComponent = levelComponent;

		if (this.levelComponent is not null)
			this.levelComponent.OnLevelChanged += _ => Refresh();
	}

	public int Strength
	{
		get => strength;
		set
		{
			if (strength == value)
				return;

			strength = value;
			Refresh();
			OnStatsChanged?.Invoke();
		}
	}

	public int Agility
	{
		get => agility;
		set
		{
			if (agility == value)
				return;

			agility = value;
			Refresh();
			OnStatsChanged?.Invoke();
		}
	}

	public int Intelligence
	{
		get => intelligence;
		set
		{
			if (intelligence == value)
				return;

			intelligence = value;
			Refresh();
			OnStatsChanged?.Invoke();
		}
	}

	public int Constitution
	{
		get => constitution;
		set
		{
			if (constitution == value)
				return;

			constitution = value;
			Refresh();
			OnStatsChanged?.Invoke();
		}
	}

	public int Perception
	{
		get => perception;
		set
		{
			if (perception == value)
				return;

			perception = value;
			Refresh();
			OnStatsChanged?.Invoke();
		}
	}

	public int Charisma
	{
		get => charisma;
		set
		{
			if (charisma == value)
				return;

			charisma = value;
			Refresh();
			OnStatsChanged?.Invoke();
		}
	}

	public event EventHandler? OnStatsChanged;

	public int GetAttribute(Attribute attribute)
	{
		return attribute switch
		{
			Attribute.Strength     => Strength,
			Attribute.Agility      => Agility,
			Attribute.Intelligence => Intelligence,
			Attribute.Constitution => Constitution,
			Attribute.Perception   => Perception,
			Attribute.Charisma     => Charisma,
			_                      => throw new Exception("Invalid attribute.")
		};
	}

	private void Refresh()
	{
		var level = levelComponent?.Level ?? 1;

		var healthBonus = 0;
		var manaBonus = 0;

		foreach (var bonus in bonuses)
		{
			healthBonus += bonus.Health;
			manaBonus += bonus.Mana;
		}

		if (healthComponent is not null)
			healthComponent.MaxHealth = healthBonus + 2 * Constitution + (level - 1) * Constitution;

		if (manaComponent is not null)
			manaComponent.MaxMana = manaBonus + Intelligence + (level - 1) * Intelligence / 2;
	}
}