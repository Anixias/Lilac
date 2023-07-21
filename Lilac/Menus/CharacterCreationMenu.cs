using System;
using System.Collections.Generic;
using System.Linq;
using Lilac.Combat;
using Lilac.Entities;
using Lilac.Items;
using Lilac.Rendering;

namespace Lilac.Menus;

public sealed class CharacterCreationMenu : MenuContainer
{
	public delegate void EventHandler();

	private readonly Character character;
	private readonly List<IMenu> menus = new();

	private int menuIndex;

	public CharacterCreationMenu(Character character)
	{
		this.character = character;

		AddMenu<PlayerNameMenu>();
		AddMenu<PlayerColorMenu>();
		AddMenu<PlayerClassMenu>();
		AddMenu<PlayerRaceMenu>();
		AddMenu<WeaponSelectMenu>();
		AddMenu<ArmorSelectMenu>();
		AddMenu<MagicQuizMenu>();

		Start();
	}

	private void AddMenu<T>() where T : ICharacterMenu, new()
	{
		T menu = new()
		{
			Character = character
		};

		menu.OnSubmitted += Advance;

		if (menu is IReturnableMenu returnableMenu)
			returnableMenu.OnBackSelected += Return;

		menus.Add(menu);
	}

	private void Start()
	{
		if (menuIndex < 0 || menuIndex >= menus.Count)
		{
			CurrentMenu = null;
			return;
		}

		CurrentMenu = menus[menuIndex];
	}

	private void Advance()
	{
		menuIndex++;

		if (menuIndex >= menus.Count)
		{
			OnCharacterFinished?.Invoke();
			CurrentMenu = null;
			return;
		}

		CurrentMenu = menus[menuIndex];
	}

	private void Return()
	{
		menuIndex = Math.Max(0, menuIndex - 1);
		CurrentMenu = menus[menuIndex];
	}

	public event EventHandler? OnCharacterFinished;

	protected override void RenderContainerTitle()
	{
		Screen.ForegroundColor = StandardColor.Blue;

		Screen.Write("# ========= ");
		Screen.ForegroundColor = StandardColor.Cyan;
		Screen.Write("Character Creation");
		Screen.ForegroundColor = StandardColor.Blue;
		Screen.WriteLine(" ========= #");

		Screen.ResetColor();
	}

	private interface ICharacterMenu : IMenu
	{
		Character Character { init; }
		event EventHandler? OnSubmitted;
	}

	private sealed class PlayerNameMenu : Prompt, ICharacterMenu
	{
		public PlayerNameMenu()
		{
			OnInputSubmitted += input =>
			{
				if (Character is not null)
					Character.Name = input;

				OnSubmitted?.Invoke();
			};
		}

		public Character? Character { get; init; }
		public event EventHandler? OnSubmitted;

		public override void RenderTitle()
		{
			Screen.WriteLine("What is your name?");
		}

		public override void Activated()
		{
			Input = Character?.Name ?? "Player";
		}
	}

	private sealed class PlayerColorMenu : Menu, IReturnableMenu, ICharacterMenu
	{
		private StandardColor selectedColor;

		public PlayerColorMenu()
		{
			selectedColor = StandardColor.Red;

			Options = new[]
			{
				new Option("Color", "Red", "Dark Red", "Yellow", "Dark Yellow", "Green", "Dark Green", "Cyan",
					"Dark Cyan", "Blue", "Dark Blue", "Magenta", "Dark Magenta")
				{
					valueChanged = value => selectedColor = value switch
					{
						0  => StandardColor.Red,
						1  => StandardColor.DarkRed,
						2  => StandardColor.Yellow,
						3  => StandardColor.DarkYellow,
						4  => StandardColor.Green,
						5  => StandardColor.DarkGreen,
						6  => StandardColor.Cyan,
						7  => StandardColor.DarkCyan,
						8  => StandardColor.Blue,
						9  => StandardColor.DarkBlue,
						10 => StandardColor.Magenta,
						11 => StandardColor.DarkMagenta,
						_  => StandardColor.White
					},
					Cycle = true
				},
				new Option("Next")
				{
					selected = () =>
					{
						if (Character is not null)
							Character.Color = selectedColor;

						OnSubmitted?.Invoke();
					}
				},
				new Option("Back")
				{
					selected = () => OnBackSelected?.Invoke()
				}
			};
		}

		public Character? Character { get; init; }

		public event CharacterCreationMenu.EventHandler? OnSubmitted;
		public event ISelectionMenu.EventHandler? OnBackSelected;

		public override void RenderTitle()
		{
			Screen.WriteLine("Select a color for your name and HUD:\n");
			Screen.ForegroundColor = selectedColor;
			Screen.WriteLine(Character?.Name ?? "Player");
			Screen.WriteLine();
		}
	}

	private sealed class PlayerClassMenu : Menu, IReturnableMenu, ICharacterMenu
	{
		private Class selectedClass;

		public PlayerClassMenu()
		{
			selectedClass = Class.Warrior;

			Options = new[]
			{
				new Option("Class", "Warrior", "Archer", "Mage")
				{
					valueChanged = value => selectedClass = value switch
					{
						0 => Class.Warrior,
						1 => Class.Archer,
						2 => Class.Mage,
						_ => throw new Exception("Invalid class id.")
					},
					Cycle = true
				},
				new Option("Next")
				{
					selected = () =>
					{
						if (Character is not null)
							Character.Class = selectedClass;

						OnSubmitted?.Invoke();
					}
				},
				new Option("Back")
				{
					selected = () => OnBackSelected?.Invoke()
				}
			};
		}

		public Character? Character { get; init; }

		public event CharacterCreationMenu.EventHandler? OnSubmitted;
		public event ISelectionMenu.EventHandler? OnBackSelected;

		public override void RenderTitle()
		{
			Screen.WriteLine("Select a class for your character:\n");
			Screen.ForegroundColor = StandardColor.DarkGray;
			Screen.WriteLine(selectedClass.Description + "\n");
			Screen.ResetColor();

			selectedClass.Bonuses?.Display();
		}
	}

	private sealed class PlayerRaceMenu : Menu, IReturnableMenu, ICharacterMenu
	{
		private Race selectedRace;

		public PlayerRaceMenu()
		{
			selectedRace = Race.Human;

			Options = new[]
			{
				new Option("Race", "Human", "Elf", "Dwarf", "Orc", "Halfling")
				{
					valueChanged = value => selectedRace = value switch
					{
						0 => Race.Human,
						1 => Race.Elf,
						2 => Race.Dwarf,
						3 => Race.Orc,
						4 => Race.Halfling,
						_ => throw new Exception("Invalid race id.")
					},
					Cycle = true
				},
				new Option("Next")
				{
					selected = () =>
					{
						if (Character is not null)
							Character.Race = selectedRace;

						OnSubmitted?.Invoke();
					}
				},
				new Option("Back")
				{
					selected = () => OnBackSelected?.Invoke()
				}
			};
		}

		public Character? Character { get; init; }

		public event CharacterCreationMenu.EventHandler? OnSubmitted;
		public event ISelectionMenu.EventHandler? OnBackSelected;

		public override void RenderTitle()
		{
			Screen.WriteLine("Select a race for your character:\n");
			Screen.ForegroundColor = StandardColor.DarkGray;
			Screen.WriteLine(selectedRace.Description + "\n");
			Screen.ResetColor();

			selectedRace.Bonuses?.Display();
			selectedRace.AttributeRolls?.Display();
		}
	}

	private sealed class WeaponSelectMenu : Menu, IReturnableMenu, ICharacterMenu
	{
		private Weapon selectedWeapon;

		public WeaponSelectMenu()
		{
			var weapons = new[]
			{
				Weapon.Saber,
				Weapon.Scimitar,
				Weapon.Claymore
			};

			selectedWeapon = weapons[0];

			Options = new[]
			{
				new Option("Weapon", weapons.Select(w => w.Name).ToArray())
				{
					valueChanged = index => selectedWeapon = weapons[index]
				},
				new Option("Next")
				{
					selected = () =>
					{
						if (Character is not null)
							Character.StartingWeapon = selectedWeapon;

						OnSubmitted?.Invoke();
					}
				},
				new Option("Back")
				{
					selected = () => OnBackSelected?.Invoke()
				}
			};
		}

		public Character? Character { get; init; }
		public event CharacterCreationMenu.EventHandler? OnSubmitted;
		public event ISelectionMenu.EventHandler? OnBackSelected;

		public override void RenderTitle()
		{
			Screen.WriteLine("Select a starting weapon type:\n");
			Screen.ForegroundColor = StandardColor.DarkGray;
			Screen.WriteLine(selectedWeapon.Description + "\n");
			Screen.ResetColor();
			Screen.WriteLine("Attribute: ".PadRight(16) + selectedWeapon.AttackAttribute);
			Screen.WriteLine("Two-Handed: ".PadRight(16) + selectedWeapon.TwoHanded);
			Screen.WriteLine("Hit Bonus: ".PadRight(16) + (selectedWeapon.HitBonus > 0 ? "+" : "") +
							 selectedWeapon.HitBonus);
			Screen.WriteLine("Damage Type: ".PadRight(16) + selectedWeapon.DamageType.DisplayName);
			Screen.WriteLine("Damage Roll: ".PadRight(16) + selectedWeapon.DamageRoll);
		}
	}

	private sealed class ArmorSelectMenu : Menu, IReturnableMenu, ICharacterMenu
	{
		private Armor selectedArmor;

		public ArmorSelectMenu()
		{
			var armors = new Armor[]
			{
				Armor.Tunic, Armor.Robes, Armor.Hide,
				Armor.Chainmail, Armor.Scale, Armor.Breastplate,
				Armor.HalfPlate, Armor.FullPlate, Armor.Spellguard
			};

			selectedArmor = armors[0];

			Options = new[]
			{
				new Option("Armor", armors.Select(w => w.Name).ToArray())
				{
					valueChanged = index => selectedArmor = armors[index]
				},
				new Option("Next")
				{
					selected = () =>
					{
						if (Character is not null)
							Character.StartingArmor = selectedArmor;

						OnSubmitted?.Invoke();
					}
				},
				new Option("Back")
				{
					selected = () => OnBackSelected?.Invoke()
				}
			};
		}

		public Character? Character { get; init; }
		public event CharacterCreationMenu.EventHandler? OnSubmitted;
		public event ISelectionMenu.EventHandler? OnBackSelected;

		public override void RenderTitle()
		{
			Screen.WriteLine("Select a starting armor type:\n");
			Screen.ForegroundColor = StandardColor.DarkGray;
			Screen.WriteLine(selectedArmor.Description + "\n");
			Screen.ResetColor();
			Screen.WriteLine("Defense: ".PadRight(16) + selectedArmor.Defenses[DamageCategory.Physical]);
			Screen.WriteLine("Resistance: ".PadRight(16) + selectedArmor.Defenses[DamageCategory.Magical]);
			Screen.WriteLine("Stealth Advantage: ".PadRight(16) + selectedArmor.StealthAdvantage);
			Screen.WriteLine("Initiative: ".PadRight(16) + selectedArmor.InitiativeBonus);
		}
	}

	private sealed class MagicQuizMenu : Menu, ICharacterMenu
	{
		private readonly Question[] questions;
		private int clockwiseScore;
		private int counterClockwiseScore;

		private int energyScore;
		private int questionIndex;
		private int spiritScore;
		private int synthesisScore;

		public MagicQuizMenu()
		{
			questions = new[]
			{
				new Question
				{
					Prompt = "When facing a difficult decision, you generally rely more on your...",
					Options = new[]
					{
						new Option("Intuition and gut feeling")
						{
							selected = () =>
							{
								energyScore++;
								Advance();
							}
						},
						new Option("Analytical skills and logic")
						{
							selected = () =>
							{
								synthesisScore++;
								Advance();
							}
						},
						new Option("Empathy and understanding of others")
						{
							selected = () =>
							{
								spiritScore++;
								Advance();
							}
						}
					}
				},
				new Question
				{
					Prompt = "How do you prefer to interact with the world around you?",
					Options = new[]
					{
						new Option("Actively, often initiating actions and making changes")
						{
							selected = () =>
							{
								energyScore++;
								Advance();
							}
						},
						new Option("Observantly, learning about and appreciating what's there")
						{
							selected = () =>
							{
								synthesisScore++;
								Advance();
							}
						},
						new Option("Harmoniously, always seeking balance and connection")
						{
							selected = () =>
							{
								spiritScore++;
								Advance();
							}
						}
					}
				},
				new Question
				{
					Prompt = "What inspires you the most?",
					Options = new[]
					{
						new Option("Action and adventure")
						{
							selected = () =>
							{
								energyScore++;
								Advance();
							}
						},
						new Option("Discovery and creation")
						{
							selected = () =>
							{
								synthesisScore++;
								Advance();
							}
						},
						new Option("Harmony and understanding")
						{
							selected = () =>
							{
								spiritScore++;
								Advance();
							}
						}
					}
				},
				new Question
				{
					Prompt = "When given a new project or task, you typically...",
					Options = new[]
					{
						new Option("Dive right in and figure things out as you go")
						{
							selected = () =>
							{
								clockwiseScore++;
								Advance();
							}
						},
						new Option("Take time to plan and organize before getting started")
						{
							selected = () =>
							{
								counterClockwiseScore++;
								Advance();
							}
						}
					}
				},
				new Question
				{
					Prompt = "When learning something new, you prefer to...",
					Options = new[]
					{
						new Option("Learn by doing, embracing trial and error")
						{
							selected = () =>
							{
								clockwiseScore++;
								Advance();
							}
						},
						new Option("Learn by researching and understanding the underlying theory")
						{
							selected = () =>
							{
								counterClockwiseScore++;
								Advance();
							}
						}
					}
				},
				new Question
				{
					Prompt = "When you face a problem, you tend to...",
					Options = new[]
					{
						new Option("Quickly make a decision and follow through")
						{
							selected = () =>
							{
								clockwiseScore++;
								Advance();
							}
						},
						new Option("Carefully consider all your options before deciding")
						{
							selected = () =>
							{
								counterClockwiseScore++;
								Advance();
							}
						}
					}
				}
			};

			Start();
		}

		public Character? Character { get; init; }

		public event CharacterCreationMenu.EventHandler? OnSubmitted;

		public override void RenderTitle()
		{
			if (questionIndex >= questions.Length)
				return;

			var question = questions[questionIndex];
			Screen.WriteLine(question.Prompt);
		}

		private void Start()
		{
			if (questionIndex >= questions.Length)
			{
				FinishQuiz();
			}
			else
			{
				var question = questions[questionIndex];
				Options = question.Options;
			}
		}

		private void Advance()
		{
			questionIndex++;
			if (questionIndex >= questions.Length)
			{
				FinishQuiz();
			}
			else
			{
				var question = questions[questionIndex];
				Options = question.Options;
			}
		}

		private void FinishQuiz()
		{
			if (Character is null)
			{
				OnSubmitted?.Invoke();
				return;
			}

			const int offset = 3;
			Character.Affinities.Add(DamageType.Fire, energyScore + clockwiseScore - offset);
			Character.Affinities.Add(DamageType.Electricity, energyScore + counterClockwiseScore - offset);
			Character.Affinities.Add(DamageType.Water, synthesisScore + clockwiseScore - offset);
			Character.Affinities.Add(DamageType.Earth, synthesisScore + counterClockwiseScore - offset);
			Character.Affinities.Add(DamageType.Shadow, spiritScore + clockwiseScore - offset);
			Character.Affinities.Add(DamageType.Air, spiritScore + counterClockwiseScore - offset);
			OnSubmitted?.Invoke();
		}

		private sealed class Question
		{
			public string Prompt { get; init; } = "";
			public Option[] Options { get; init; } = Array.Empty<Option>();
		}
	}
}