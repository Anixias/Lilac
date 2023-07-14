using System;
using Lilac.Entities;

namespace Lilac.Menus;

public sealed class CharacterCreationMenu : MenuContainer
{
    public delegate void EventHandler();
    public event EventHandler? OnCharacterFinished;

    public CharacterCreationMenu(Character character)
    {
        var playerNameMenu = new PlayerNameMenu(character.Name);
        playerNameMenu.OnInputSubmitted += (input) =>
        {
            character.Name = input;

            var playerColorMenu = new PlayerColorMenu(character.Name);
            playerColorMenu.OnColorSelected += color =>
            {
                character.Color = color;

                var playerClassMenu = new PlayerClassMenu();
                playerClassMenu.OnClassSelected += @class =>
                {
                    character.Class = @class;

                    var playerRaceMenu = new PlayerRaceMenu();
                    playerRaceMenu.OnRaceSelected += race =>
                    {
                        character.Race = race;
                        OnCharacterFinished?.Invoke();
                    };

                    playerRaceMenu.OnBackSelected += () =>
                    {
                        currentMenu = playerClassMenu;
                    };

                    currentMenu = playerRaceMenu;
                };

                playerClassMenu.OnBackSelected += () =>
                {
                    currentMenu = playerColorMenu;
                };

                currentMenu = playerClassMenu;
            };

			playerColorMenu.OnBackSelected += () =>
			{
				playerNameMenu.SetInput(character.Name);
				currentMenu = playerNameMenu;
			};

            currentMenu = playerColorMenu;
        };

        currentMenu = playerNameMenu;
    }

    protected override void RenderContainerTitle()
    {
        Console.ForegroundColor = ConsoleColor.Blue;

        Console.Write("# ========= ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("Character Creation");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(" ========= #");

        Console.ResetColor();
    }

    private sealed class PlayerNameMenu : Prompt
    {
        public PlayerNameMenu(string existingName)
        {
            Input = existingName;
        }

        public override void RenderTitle()
        {
            Console.WriteLine("What is your name?");
        }
    }

    private sealed class PlayerColorMenu : Menu
    {
        public delegate void ColorEventHandler(ConsoleColor color);
        public event ColorEventHandler? OnColorSelected;
        public event EventHandler? OnBackSelected;

        private ConsoleColor selectedColor;
        private readonly string displayName;

        public PlayerColorMenu(string characterName)
        {
            displayName = characterName;
            selectedColor = ConsoleColor.Red;

            Options = new[]
            {
                new Option("Color", "Red", "Dark Red", "Yellow", "Dark Yellow", "Green", "Dark Green", "Cyan", "Dark Cyan", "Blue", "Dark Blue", "Magenta", "Dark Magenta")
                {
                    valueChanged = value => selectedColor = value switch
                    {
                        0 => ConsoleColor.Red,
                        1 => ConsoleColor.DarkRed,
                        2 => ConsoleColor.Yellow,
                        3 => ConsoleColor.DarkYellow,
                        4 => ConsoleColor.Green,
                        5 => ConsoleColor.DarkGreen,
                        6 => ConsoleColor.Cyan,
                        7 => ConsoleColor.DarkCyan,
                        8 => ConsoleColor.Blue,
                        9 => ConsoleColor.DarkBlue,
                        10 => ConsoleColor.Magenta,
                        11 => ConsoleColor.DarkMagenta,
                        _ => ConsoleColor.White
                    }
                },
                new Option("Next")
                {
                    selected = () => OnColorSelected?.Invoke(selectedColor)
                },
                new Option("Back")
                {
                    selected = () => OnBackSelected?.Invoke()
                }
            };
        }

        public override void RenderTitle()
        {
            Console.WriteLine("Select a color for your name and HUD:\n");
            Console.ForegroundColor = selectedColor;
            Console.WriteLine(displayName);
            Console.WriteLine();
        }
    }

    private sealed class PlayerClassMenu : Menu
    {
        public delegate void ClassEventHandler(Class @class);
        public event ClassEventHandler? OnClassSelected;
        public event EventHandler? OnBackSelected;

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
                    }
                },
                new Option("Next")
                {
                    selected = () => OnClassSelected?.Invoke(selectedClass)
                },
                new Option("Back")
                {
                    selected = () => OnBackSelected?.Invoke()
                }
            };
        }

        public override void RenderTitle()
        {
            Console.WriteLine("Select a class for your character:\n");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(selectedClass.Description + "\n");
            Console.ResetColor();

			selectedClass.Bonuses?.Display();
        }
    }

    private sealed class PlayerRaceMenu : Menu
    {
        public delegate void RaceEventHandler(Race race);
        public event RaceEventHandler? OnRaceSelected;
        public event EventHandler? OnBackSelected;

        private Race selectedRace;

        public PlayerRaceMenu()
        {
            selectedRace = Race.Human;

            Options = new[]
            {
                new Option("Race", "Human", "Elf", "Dwarf", "Orc", "Halfling")
                {
                    valueChanged = (value) => selectedRace = value switch
                    {
                        0 => Race.Human,
                        1 => Race.Elf,
                        2 => Race.Dwarf,
                        3 => Race.Orc,
                        4 => Race.Halfling,
                        _ => throw new Exception("Invalid race id.")
                    }
                },
                new Option("Next")
                {
                    selected = () => OnRaceSelected?.Invoke(selectedRace)
                },
                new Option("Back")
                {
                    selected = () => OnBackSelected?.Invoke()
                }
            };
        }

        public override void RenderTitle()
        {
            Console.WriteLine("Select a race for your character:\n");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(selectedRace.Description + "\n");
            Console.ResetColor();

			selectedRace.Bonuses?.Display();
			selectedRace.AttributeRolls?.Display();
        }
    }
}