using System;
using Lilac.Dice;
using Lilac.Rendering;

namespace Lilac.Entities;

public sealed class AttributeRolls
{
	private readonly Roll? strength;
	private readonly Roll? agility;
	private readonly Roll? intelligence;
	private readonly Roll? constitution;
	private readonly Roll? perception;
	private readonly Roll? charisma;
	
	private readonly (IRollResult, IRollResult)? strengthRange;
	private readonly (IRollResult, IRollResult)? agilityRange;
	private readonly (IRollResult, IRollResult)? intelligenceRange;
	private readonly (IRollResult, IRollResult)? constitutionRange;
	private readonly (IRollResult, IRollResult)? perceptionRange;
	private readonly (IRollResult, IRollResult)? charismaRange;
	
	public Roll? Strength
	{
		get => strength;
		init
		{
			strength = value;
			strengthRange = strength?.Range();
		}
	}
	
	public Roll? Agility
	{
		get => agility;
		init
		{
			agility = value;
			agilityRange = agility?.Range();
		}
	}
	
	public Roll? Intelligence
	{
		get => intelligence;
		init
		{
			intelligence = value;
			intelligenceRange = intelligence?.Range();
		}
	}
	
	public Roll? Constitution
	{
		get => constitution;
		init
		{
			constitution = value;
			constitutionRange = constitution?.Range();
		}
	}
	
	public Roll? Perception
	{
		get => perception;
		init
		{
			perception = value;
			perceptionRange = perception?.Range();
		}
	}
	
	public Roll? Charisma
	{
		get => charisma;
		init
		{
			charisma = value;
			charismaRange = charisma?.Range();
		}
	}
	

	public void Display()
	{
		Screen.Write("Strength: ".PadRight(16));
		if (strength is not null)
		{
			Screen.ForegroundColor = StandardColor.DarkBlue;
			Screen.Write(strength);
			Screen.ResetColor();
		}
		if (strengthRange is not null)
		{
			Screen.ForegroundColor = StandardColor.DarkGray;
			Screen.Write($" ({strengthRange?.Item1.Value}-{strengthRange?.Item2.Value})");
			Screen.ResetColor();
		}
		Screen.WriteLine();
		
		Screen.Write("Agility: ".PadRight(16));
		if (agility is not null)
		{
			Screen.ForegroundColor = StandardColor.DarkBlue;
			Screen.Write(agility);
			Screen.ResetColor();
		}
		if (agilityRange is not null)
		{
			Screen.ForegroundColor = StandardColor.DarkGray;
			Screen.Write($" ({agilityRange?.Item1.Value}-{agilityRange?.Item2.Value})");
			Screen.ResetColor();
		}
		Screen.WriteLine();
		
		Screen.Write("Intelligence: ".PadRight(16));
		if (intelligence is not null)
		{
			Screen.ForegroundColor = StandardColor.DarkBlue;
			Screen.Write(intelligence);
			Screen.ResetColor();
		}
		if (intelligenceRange is not null)
		{
			Screen.ForegroundColor = StandardColor.DarkGray;
			Screen.Write($" ({intelligenceRange?.Item1.Value}-{intelligenceRange?.Item2.Value})");
			Screen.ResetColor();
		}
		Screen.WriteLine();
		
		Screen.Write("Constitution: ".PadRight(16));
		if (constitution is not null)
		{
			Screen.ForegroundColor = StandardColor.DarkBlue;
			Screen.Write(constitution);
			Screen.ResetColor();
		}
		if (constitutionRange is not null)
		{
			Screen.ForegroundColor = StandardColor.DarkGray;
			Screen.Write($" ({constitutionRange?.Item1.Value}-{constitutionRange?.Item2.Value})");
			Screen.ResetColor();
		}
		Screen.WriteLine();
		
		Screen.Write("Perception: ".PadRight(16));
		if (perception is not null)
		{
			Screen.ForegroundColor = StandardColor.DarkBlue;
			Screen.Write(perception);
			Screen.ResetColor();
		}
		if (perceptionRange is not null)
		{
			Screen.ForegroundColor = StandardColor.DarkGray;
			Screen.Write($" ({perceptionRange?.Item1.Value}-{perceptionRange?.Item2.Value})");
			Screen.ResetColor();
		}
		Screen.WriteLine();
		
		Screen.Write("Charisma: ".PadRight(16));
		if (charisma is not null)
		{
			Screen.ForegroundColor = StandardColor.DarkBlue;
			Screen.Write(charisma);
			Screen.ResetColor();
		}
		if (charismaRange is not null)
		{
			Screen.ForegroundColor = StandardColor.DarkGray;
			Screen.Write($" ({charismaRange?.Item1.Value}-{charismaRange?.Item2.Value})");
			Screen.ResetColor();
		}
		Screen.WriteLine("\n");
	}
}