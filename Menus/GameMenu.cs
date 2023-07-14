using System;
using System.Collections.Generic;
using System.Linq;
using Lilac.Components;
using Lilac.Rendering;
using Lilac.Entities.Creatures;  
using Lilac.Maps;
using Lilac.Combat;

namespace Lilac.Menus;

public sealed class GameMenu : MenuContainer
{
	private bool showControls;
	
	public GameMenu()
	{
		customKeyEvents.Add(new ConsoleKeyInfo('i', ConsoleKey.I, false, true, false), () =>
		{
			showControls = !showControls;
		});
		
		customKeyEvents.Add(new ConsoleKeyInfo('p', ConsoleKey.P, false, true, false), ShowPartyInformation);

		var battle = new Battle();
		if (Game.Singleton is { } game)
		{
			foreach (var partyMember in game.Party)
				battle.AddBattleMember(partyMember);
		}

		battle.AddBattleMember(new GiantRat {Name = "Giant Rat 1"});
		battle.AddBattleMember(new GiantRat {Name = "Giant Rat 2"});
		battle.AddBattleMember(new GiantRat {Name = "Giant Rat 3"});

		battle.Begin();
		var battleMenu = new BattleMenu(battle);
		currentMenu = battleMenu;
	}

	private void ShowPartyInformation()
	{
		if (currentMenu is CharacterInformationMenu)
			return;
		
		var previousMenu = currentMenu;
		
		var characterInformationMenu = new CharacterInformationMenu();
		characterInformationMenu.OnContinueSelected += () =>
		{
			currentMenu = previousMenu;
		};
		
		currentMenu = characterInformationMenu;
	}
	
	protected override void RenderContainerTitle()
    {
		if (currentMenu is CharacterInformationMenu)
			return;
		
		var player = Game.Singleton?.Player;

		if (player is not null)
		{
			player.Render();
			Console.WriteLine($" - Level {player.Level} {player.Character.Race.DisplayName} {player.Character.Class.DisplayName}");

			Console.Write($"XP: {player.XP}/{player.MaxXP} ".PadRight(16));
			Drawing.DrawBar(16, player.XP / (double)player.MaxXP, ConsoleColor.Yellow);
			Console.WriteLine();
			
			Console.Write($"HP: {player.Health}/{player.MaxHealth} ".PadRight(16));
			Drawing.DrawBar(16, player.Health / (double)player.MaxHealth, ConsoleColor.DarkRed);
			Console.WriteLine();
			
			Console.Write($"MP: {player.Mana}/{player.MaxMana} ".PadRight(16));
			Drawing.DrawBar(16, player.Mana / (double)player.MaxMana, ConsoleColor.DarkBlue);
			Console.WriteLine();
			
			Console.WriteLine();

			const int controlPadding = 12;
			if (showControls)
			{
				Console.WriteLine("Alt + I".PadRight(controlPadding) + "Toggle controls");
				Console.WriteLine("- Escape".PadRight(controlPadding) + "Toggle pause menu");
				Console.WriteLine("- Alt + P".PadRight(controlPadding) + "Party Info");
				Console.WriteLine("- Tab".PadRight(controlPadding) + "Inventory");
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.WriteLine("Alt + I".PadRight(controlPadding) + "Toggle controls");
				Console.ResetColor();
			}
		}
    }

    private sealed class CharacterInformationMenu : Menu
    {
		public event EventHandler? OnContinueSelected;

		private Creature? selectedMember;

		private enum InfoPage
		{
			Attributes,
			Combat
		}

		private readonly string[] pageTitles = 
		{
			"Attributes", "Combat"
		};

		private InfoPage selectedPage = InfoPage.Attributes;
		
		public CharacterInformationMenu()
		{
			if (Game.Singleton is null)
				return;

			if (Game.Singleton.Party.Count > 0)
				selectedMember = Game.Singleton.Party[0];
			
			var characters = new string[Game.Singleton.Party.Count];

			for (var i = 0; i < characters.Length; i++)
			{
				characters[i] = Game.Singleton.Party[i].Name;
			}
			
			Options = new[]
			{
				new Option("Page", pageTitles)
				{
					valueChanged = (index) =>
					{
						selectedPage = (InfoPage)index;
					}
				},
				new Option("Party Member", characters)
				{
					valueChanged = (index) => 
					{
						selectedMember = Game.Singleton.Party[index];
					}
				},
				new Option("Continue")
				{
					selected = () => OnContinueSelected?.Invoke()
				}
			};
		}
		
        public override void RenderTitle()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
	        Console.Write("# ========= ");
	        Console.ForegroundColor = ConsoleColor.Cyan;
	        Console.Write("Character Information");
	        Console.ForegroundColor = ConsoleColor.DarkBlue;
	        Console.WriteLine(" ========= #");
			Console.ResetColor();

			// Render selected party member
			if (selectedMember is null)
				return;
			
			selectedMember.Render();
			if (selectedMember.GetComponent<CharacterComponent>() is { } characterComponent)
			{
				Console.WriteLine($" - Level {selectedMember.Level} {selectedMember.Species} {characterComponent.Character.Class.DisplayName}");
			}
			else
			{
				Console.WriteLine($" - Level {selectedMember.Level} {selectedMember.Species}");
			}

			Console.Write($"XP: {selectedMember.XP}/{selectedMember.MaxXP} ".PadRight(16));
			Drawing.DrawBar(16, selectedMember.XP / (double)selectedMember.MaxXP, ConsoleColor.Yellow);
			Console.WriteLine();
			
			Console.Write($"HP: {selectedMember.Health}/{selectedMember.MaxHealth} ".PadRight(16));
			Drawing.DrawBar(16, selectedMember.Health / (double)selectedMember.MaxHealth, ConsoleColor.DarkRed);
			Console.WriteLine();
			
			Console.Write($"MP: {selectedMember.Mana}/{selectedMember.MaxMana} ".PadRight(16));
			Drawing.DrawBar(16, selectedMember.Mana / (double)selectedMember.MaxMana, ConsoleColor.DarkBlue);
			Console.WriteLine();
			Console.WriteLine();
	
            Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write("# ========= ");
	        Console.ForegroundColor = ConsoleColor.DarkMagenta;
	        Console.Write(pageTitles[(int)selectedPage]);
	        Console.ForegroundColor = ConsoleColor.DarkGray;
	        Console.WriteLine(" ========= #");
	        Console.ResetColor();

			switch (selectedPage)
			{
				case InfoPage.Attributes:
					if (selectedMember.GetComponent<StatsComponent>() is not { } statsComponent)
						break;
					
					Console.WriteLine();
					Console.ResetColor();
	
					const ConsoleColor attributeColor = ConsoleColor.DarkBlue;
					
					Console.Write("Strength: ".PadRight(16));
					Console.ForegroundColor = attributeColor;
					Console.WriteLine(statsComponent.Strength);
					Console.ResetColor();
					
					Console.Write("Agility: ".PadRight(16));
					Console.ForegroundColor = attributeColor;
					Console.WriteLine(statsComponent.Agility);
					Console.ResetColor();
					
					Console.Write("Intelligence: ".PadRight(16));
					Console.ForegroundColor = attributeColor;
					Console.WriteLine(statsComponent.Intelligence);
					Console.ResetColor();
					
					Console.Write("Constitution: ".PadRight(16));
					Console.ForegroundColor = attributeColor;
					Console.WriteLine(statsComponent.Constitution);
					Console.ResetColor();
					
					Console.Write("Perception: ".PadRight(16));
					Console.ForegroundColor = attributeColor;
					Console.WriteLine(statsComponent.Perception);
					Console.ResetColor();
					
					Console.Write("Charisma: ".PadRight(16));
					Console.ForegroundColor = attributeColor;
					Console.WriteLine(statsComponent.Charisma);
					Console.ResetColor();
						
					Console.WriteLine();
					break;
				case InfoPage.Combat:
					if (selectedMember.GetComponent<CombatComponent>() is not { } combatComponent)
						break;
					
					Console.WriteLine();
					Console.ResetColor();
	
					const ConsoleColor combatStatColor = ConsoleColor.Green;
					
					Console.Write("Initiative: ".PadRight(24));
					Console.ForegroundColor = combatStatColor;
					Console.WriteLine(combatComponent.Initiative);
					Console.ResetColor();
					
					Console.Write("Evasion: ".PadRight(24));
					Console.ForegroundColor = combatStatColor;
					Console.WriteLine(combatComponent.Evasion);
					Console.ResetColor();

					// Defenses
					foreach (var physicalDamageType in DamageType.PhysicalTypes)
					{
						Console.Write($"{physicalDamageType.DisplayName} Defense: ".PadRight(24));
						Console.ForegroundColor = combatStatColor;
						Console.WriteLine(combatComponent.GetDefense(physicalDamageType));
						Console.ResetColor();
					}

					// Resistances
					foreach (var magicalDamageType in DamageType.MagicalTypes)
					{
						Console.Write($"{magicalDamageType.DisplayName} Resistance: ".PadRight(24));
						Console.ForegroundColor = combatStatColor;
						Console.WriteLine(combatComponent.GetResistance(magicalDamageType));
						Console.ResetColor();
					}
				
					Console.WriteLine();
					break;
			}
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
            Console.ForegroundColor = Tile.Map.SecondaryColor;
	        Console.Write("# ========= ");
	        Console.ForegroundColor = Tile.Map.PrimaryColor;
	        Console.Write($"{Tile.Map.Name} - {Tile.Name}");
	        Console.ForegroundColor = Tile.Map.SecondaryColor;
	        Console.WriteLine(" ========= #");
	        Console.ResetColor();

			Console.WriteLine(Tile.Description);
        }
    }

	private sealed class BattleMenu : Menu
	{
		private Page currentPage;
		private Page? previousPage;
		
		private enum Page
		{
			Main,
			Attack
		}
		
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
			Console.ForegroundColor = ConsoleColor.DarkGray;
	        Console.Write("# ========= ");
	        Console.ForegroundColor = ConsoleColor.DarkRed;
	        Console.Write("Battle");
	        Console.ForegroundColor = ConsoleColor.DarkGray;
	        Console.WriteLine(" ========= #");
			Console.ResetColor();

			Console.WriteLine();
			Battle.Render();
			Console.WriteLine();
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
					{
						foreach (var battleMember in Battle.BattleMembers)
						{
							if (battleMember == currentBattleMember)
								continue;

							if (battleMember.IsDead)
								continue;
							
							if (battleMember is not IHittable hittable)
								continue;
							
							if (currentBattleMember.GetRelationship(battleMember.Allegiances.ToArray<IRelationship>()) != RelationshipState.Enemy)
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
					}

					newOptions.Add(new Option("Cancel")
					{
						selected = () => currentPage = Page.Main
					});
					Options = newOptions.ToArray();
					break;
			}
		}
	}
}