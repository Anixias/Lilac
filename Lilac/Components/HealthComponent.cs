using System;

namespace Lilac.Components;

public sealed class HealthComponent : IComponent
{
	private int health;
	private int maxHealth;

	public HealthComponent()
	{
	}

	public HealthComponent(int maxHealth)
	{
		MaxHealth = maxHealth;
		Health = maxHealth;
	}

	public int Health
	{
		get => health;
		set => health = Math.Clamp(value, 0, MaxHealth);
	}

	public int MaxHealth
	{
		get => maxHealth;
		set
		{
			var prevMaxHealth = maxHealth;
			maxHealth = Math.Max(0, value);

			health = maxHealth < prevMaxHealth
				? Math.Min(health, maxHealth)
				: Math.Min(health + maxHealth - prevMaxHealth, maxHealth);
		}
	}

	public double Percent => MaxHealth != 0 ? Health / (double)MaxHealth : 0.0;
}