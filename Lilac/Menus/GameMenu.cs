using System;
using System.Collections.Generic;
using System.Linq;
using Lilac.Combat;
using Lilac.Components;
using Lilac.Entities.Creatures;
using Lilac.Maps;
using Lilac.Rendering;

namespace Lilac.Menus;

public sealed class GameMenu : MenuContainer
{
	private bool showControls;

	public GameMenu()
	{
		customKeyEvents.Add(new ConsoleKeyInfo('i', ConsoleKey.I, false, true, false),
			() => { showControls = !showControls; });

		customKeyEvents.Add(new ConsoleKeyInfo('p', ConsoleKey.P, false, true, false), ShowPartyInformation);
		customKeyEvents.Add(new ConsoleKeyInfo('\t', ConsoleKey.Tab, false, false, false), ShowInventory);
	}

	private void ShowPartyInformation()
	{
		if (CurrentMenu is CharacterInformationMenu)
			return;

		var previousMenu = CurrentMenu;

		var characterInformationMenu = new CharacterInformationMenu();
		characterInformationMenu.OnContinueSelected += () => CurrentMenu = previousMenu;

		CurrentMenu = characterInformationMenu;
	}

	private void ShowInventory()
	{
		if (CurrentMenu is InventoryMenu)
			return;

		var previousMenu = CurrentMenu;

		var inventoryMenu = new InventoryMenu();
		inventoryMenu.OnContinueSelected += () => CurrentMenu = previousMenu;

		CurrentMenu = inventoryMenu;
	}

	protected override void RenderContainerTitle()
	{
		if (CurrentMenu is CharacterInformationMenu)
			return;

		var player = Game.Singleton?.Player;

		if (player is not null)
		{
			player.Render();
			Screen.WriteLine(
				$" - Level {player.Level} {player.Character.Race.DisplayName} {player.Character.Class.DisplayName}");

			Screen.Write($"XP: {player.XP}/{player.MaxXP} ".PadRight(16));
			Drawing.DrawBar(16, player.XP / (double)player.MaxXP, StandardColor.Yellow);
			Screen.WriteLine();

			Screen.Write($"HP: {player.Health}/{player.MaxHealth} ".PadRight(16));
			Drawing.DrawBar(16, player.Health / (double)player.MaxHealth, StandardColor.DarkRed);
			Screen.WriteLine();

			Screen.Write($"MP: {player.Mana}/{player.MaxMana} ".PadRight(16));
			Drawing.DrawBar(16, player.Mana / (double)player.MaxMana, StandardColor.DarkBlue);
			Screen.WriteLine();

			Screen.WriteLine();

			const int controlPadding = 12;
			if (showControls)
			{
				Screen.WriteLine("Alt + I".PadRight(controlPadding) + "Toggle controls");
				Screen.WriteLine("- Escape".PadRight(controlPadding) + "Toggle pause menu");
				Screen.WriteLine("- Alt + P".PadRight(controlPadding) + "Party Info");
				Screen.WriteLine("- Tab".PadRight(controlPadding) + "Inventory");
			}
			else
			{
				Screen.ForegroundColor = StandardColor.DarkGray;
				Screen.WriteLine("Alt + I".PadRight(controlPadding) + "Toggle controls");
				Screen.ResetColor();
			}
		}
	}

	private sealed class CharacterInformationMenu : Menu
	{
		private readonly string[] pageTitles =
		{
			"Attributes", "Combat"
		};

		private Creature? selectedMember;

		private InfoPage selectedPage = InfoPage.Attributes;

		public CharacterInformationMenu()
		{
			if (Game.Singleton is null)
				return;

			if (Game.Singleton.Party.Count > 0)
				selectedMember = Game.Singleton.Party[0];

			var characters = new string[Game.Singleton.Party.Count];

			for (var i = 0; i < characters.Length; i++) characters[i] = Game.Singleton.Party[i].Name;

			Options = new[]
			{
				new Option("Page", pageTitles)
				{
					valueChanged = index => { selectedPage = (InfoPage)index; }
				},
				new Option("Party Member", characters)
				{
					valueChanged = index => { selectedMember = Game.Singleton.Party[index]; }
				},
				new Option("Continue")
				{
					selected = () => OnContinueSelected?.Invoke()
				}
			};
		}

		public event EventHandler? OnContinueSelected;

		public override void RenderTitle()
		{
			Screen.ForegroundColor = StandardColor.DarkBlue;
			Screen.Write("# ========= ");
			Screen.ForegroundColor = StandardColor.Cyan;
			Screen.Write("Character Information");
			Screen.ForegroundColor = StandardColor.DarkBlue;
			Screen.WriteLine(" ========= #");
			Screen.ResetColor();

			// Render selected party member
			if (selectedMember is null)
				return;

			selectedMember.Render();
			if (selectedMember.GetComponent<CharacterComponent>() is { } characterComponent)
				Screen.WriteLine(
					$" - Level {selectedMember.Level} {selectedMember.Species} {characterComponent.Character.Class.DisplayName}");
			else
				Screen.WriteLine($" - Level {selectedMember.Level} {selectedMember.Species}");

			Screen.Write($"XP: {selectedMember.XP}/{selectedMember.MaxXP} ".PadRight(16));
			Drawing.DrawBar(16, selectedMember.XP / (double)selectedMember.MaxXP, StandardColor.Yellow);
			Screen.WriteLine();

			Screen.Write($"HP: {selectedMember.Health}/{selectedMember.MaxHealth} ".PadRight(16));
			Drawing.DrawBar(16, selectedMember.Health / (double)selectedMember.MaxHealth, StandardColor.DarkRed);
			Screen.WriteLine();

			Screen.Write($"MP: {selectedMember.Mana}/{selectedMember.MaxMana} ".PadRight(16));
			Drawing.DrawBar(16, selectedMember.Mana / (double)selectedMember.MaxMana, StandardColor.DarkBlue);
			Screen.WriteLine();
			Screen.WriteLine();

			Screen.ForegroundColor = StandardColor.DarkGray;
			Screen.Write("# ========= ");
			Screen.ForegroundColor = StandardColor.DarkMagenta;
			Screen.Write(pageTitles[(int)selectedPage]);
			Screen.ForegroundColor = StandardColor.DarkGray;
			Screen.WriteLine(" ========= #");
			Screen.ResetColor();

			switch (selectedPage)
			{
				case InfoPage.Attributes:
					if (selectedMember.GetComponent<StatsComponent>() is not { } statsComponent)
						break;

					Screen.WriteLine();
					Screen.ResetColor();

					var attributeColor = StandardColor.DarkBlue;

					Screen.Write("Strength: ".PadRight(16));
					Screen.ForegroundColor = attributeColor;
					Screen.WriteLine(statsComponent.Strength);
					Screen.ResetColor();

					Screen.Write("Agility: ".PadRight(16));
					Screen.ForegroundColor = attributeColor;
					Screen.WriteLine(statsComponent.Agility);
					Screen.ResetColor();

					Screen.Write("Intelligence: ".PadRight(16));
					Screen.ForegroundColor = attributeColor;
					Screen.WriteLine(statsComponent.Intelligence);
					Screen.ResetColor();

					Screen.Write("Constitution: ".PadRight(16));
					Screen.ForegroundColor = attributeColor;
					Screen.WriteLine(statsComponent.Constitution);
					Screen.ResetColor();

					Screen.Write("Perception: ".PadRight(16));
					Screen.ForegroundColor = attributeColor;
					Screen.WriteLine(statsComponent.Perception);
					Screen.ResetColor();

					Screen.Write("Charisma: ".PadRight(16));
					Screen.ForegroundColor = attributeColor;
					Screen.WriteLine(statsComponent.Charisma);
					Screen.ResetColor();

					Screen.WriteLine();
					break;
				case InfoPage.Combat:
					if (selectedMember.GetComponent<CombatComponent>() is not { } combatComponent)
						break;

					Screen.WriteLine();
					Screen.ResetColor();

					var combatStatColor = StandardColor.Green;

					Screen.Write("Initiative: ".PadRight(24));
					Screen.ForegroundColor = combatStatColor;
					Screen.WriteLine(combatComponent.Initiative);
					Screen.ResetColor();

					Screen.Write("Evasion: ".PadRight(24));
					Screen.ForegroundColor = combatStatColor;
					Screen.WriteLine(combatComponent.Evasion);
					Screen.ResetColor();

					// Defenses
					foreach (var physicalDamageType in DamageType.PhysicalTypes)
					{
						Screen.Write($"{physicalDamageType.DisplayName} Defense: ".PadRight(24));
						Screen.ForegroundColor = combatStatColor;
						Screen.WriteLine(combatComponent.GetDefense(physicalDamageType));
						Screen.ResetColor();
					}

					// Resistances
					foreach (var magicalDamageType in DamageType.MagicalTypes)
					{
						Screen.Write($"{magicalDamageType.DisplayName} Resistance: ".PadRight(24));
						Screen.ForegroundColor = combatStatColor;
						Screen.WriteLine(combatComponent.GetResistance(magicalDamageType));
						Screen.ResetColor();
					}

					Screen.WriteLine();
					break;
			}
		}

		private enum InfoPage
		{
			Attributes,
			Combat
		}
	}

	private sealed class InventoryMenu : Menu
	{
		private readonly Page equipmentPage = new("Equipment");
		private readonly Page inventoryPage = new("Inventory");
		private Creature? selectedMember;
		private Page selectedPage;

		public InventoryMenu()
		{
			if (Game.Singleton is null)
			{
				selectedPage = inventoryPage;
				return;
			}

			if (Game.Singleton.Party.Count > 0)
				selectedMember = Game.Singleton.Party[0];

			var characters = new string[Game.Singleton.Party.Count];

			for (var i = 0; i < characters.Length; i++) characters[i] = Game.Singleton.Party[i].Name;

			inventoryPage.Options = new[]
			{
				new Option("Party Member", characters)
				{
					valueChanged = index => { selectedMember = Game.Singleton.Party[index]; }
				},
				new Option("Manage Equipment")
				{
					selected = () => { SelectedPage = equipmentPage; }
				},
				new Option("Continue")
				{
					selected = () => OnContinueSelected?.Invoke()
				}
			};

			selectedPage = inventoryPage;
			Options = selectedPage.Options;
		}

		private Page SelectedPage
		{
			get => selectedPage;
			set
			{
				selectedPage = value;
				Options = selectedPage.Options;
			}
		}

		public event EventHandler? OnContinueSelected;

		public override void RenderTitle()
		{
			Screen.ForegroundColor = StandardColor.DarkGreen;
			Screen.Write("# ========= ");
			Screen.ForegroundColor = StandardColor.Blue;
			Screen.Write(SelectedPage.Title);
			Screen.ForegroundColor = StandardColor.DarkGreen;
			Screen.WriteLine(" ========= #");
			Screen.ResetColor();
		}

		private sealed class Page
		{
			public Page(string title)
			{
				Title = title;
			}

			public string Title { get; }
			public Option[] Options { get; set; } = Array.Empty<Option>();
		}
	}

	private sealed class TileMenu : Menu
	{
		public TileMenu(ITile tile)
		{
			Tile = tile;
		}

		public ITile Tile { get; }

		public override void RenderTitle()
		{
			Screen.ForegroundColor = Tile.Map.SecondaryColor;
			Screen.Write("# ========= ");
			Screen.ForegroundColor = Tile.Map.PrimaryColor;
			Screen.Write($"{Tile.Map.Name} - {Tile.Name}");
			Screen.ForegroundColor = Tile.Map.SecondaryColor;
			Screen.WriteLine(" ========= #");
			Screen.ResetColor();

			Screen.WriteLine(Tile.Description);
		}
	}

	private sealed class BattleMenu : Menu
	{
		private Page currentPage;
		private Page? previousPage;

		public BattleMenu(Battle battle)
		{
			Battle = battle;
			battle.OnTurnChanged += () =>
			{
				currentPage = Page.Main;
				UpdateOptions();
			};
		}

		private Battle Battle { get; }

		public override void RenderTitle()
		{
			Screen.ForegroundColor = StandardColor.DarkGray;
			Screen.Write("# ========= ");
			Screen.ForegroundColor = StandardColor.DarkRed;
			Screen.Write("Battle");
			Screen.ForegroundColor = StandardColor.DarkGray;
			Screen.WriteLine(" ========= #");
			Screen.ResetColor();

			Screen.WriteLine();
			Battle.Render();
			Screen.WriteLine();
		}

		public override void RenderOptions()
		{
			if (previousPage is null || previousPage != currentPage)
			{
				UpdateOptions();
				previousPage = currentPage;
			}

			base.RenderOptions();
		}

		private void UpdateOptions()
		{
			Options = new[]
			{
				new Option("Next")
				{
					selected = () => Battle.CurrentBattleMember?.TakeTurn()
				}
			};

			if (Battle.CurrentBattleMember is not { } currentBattleMember)
				return;

			if (!currentBattleMember.IsUser)
				return;

			switch (currentPage)
			{
				case Page.Main:
					Options = new[]
					{
						new Option("Attack")
						{
							selected = () => currentPage = Page.Attack
						},
						new Option("Evade"),
						new Option("Hide"),
						new Option("Prepare"),
						new Option("Use"),
						new Option("Cast"),
						new Option("Skip")
						{
							selected = () => currentBattleMember.EndTurn()
						},
						new Option("Flee")
					};
					break;
				case Page.Attack:
					var newOptions = new List<Option>();
					if (currentBattleMember is IAttacker attacker)
						foreach (var battleMember in Battle.BattleMembers)
						{
							if (battleMember == currentBattleMember)
								continue;

							if (battleMember.IsDead)
								continue;

							if (battleMember is not IHittable hittable)
								continue;

							if (currentBattleMember.GetRelationship(battleMember.Allegiances
									.ToArray<IRelationship>()) != RelationshipState.Enemy)
								continue;

							newOptions.Add(new Option(hittable.Name)
							{
								selected = () =>
								{
									attacker.Attack(hittable);
									currentBattleMember.EndTurn();
								}
							});
						}

					newOptions.Add(new Option("Cancel")
					{
						selected = () => currentPage = Page.Main
					});
					Options = newOptions.ToArray();
					break;
			}
		}

		private enum Page
		{
			Main,
			Attack
		}
	}
}