using System;

namespace Lilac.Components;

public sealed class ManaComponent : IComponent
{
	private int mana;
	private int maxMana;

	public ManaComponent()
	{
		
	}

	public ManaComponent(int maxMana)
	{
		MaxMana = maxMana;
		Mana = maxMana;
	}
	
	public int Mana
	{
		get => mana;
		set => mana = Math.Clamp(value, 0, MaxMana);
	}
	
	public int MaxMana
	{
		get => maxMana;
		set
		{
			var prevMaxMana = maxMana;
			maxMana = Math.Max(0, value);
			mana = Math.Min(mana, maxMana);

			if (maxMana < prevMaxMana)
				mana = Math.Min(mana, maxMana);
			else
				mana = Math.Min(mana + maxMana - prevMaxMana, maxMana);
		}
	}
}