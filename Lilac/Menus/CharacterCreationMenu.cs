using System;
using Lilac.Entities;
using Lilac.Rendering;

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
        Screen.ForegroundColor = StandardColor.Blue;

        Screen.Write("# ========= ");
        Screen.ForegroundColor = StandardColor.Cyan;
        Screen.Write("Character Creation");
        Screen.ForegroundColor = StandardColor.Blue;
        Screen.WriteLine(" ========= #");

        Screen.ResetColor();
    }

    private sealed class PlayerNameMenu : Prompt
    {
        public PlayerNameMenu(string existingName)
        {
            Input = existingName;
        }

        public override void RenderTitle()
        {
            Screen.WriteLine("What is your name?");
        }
    }

    private sealed class PlayerColorMenu : Menu
    {
        public delegate void ColorEventHandler(IColor color);
        public event ColorEventHandler? OnColorSelected;
        public event EventHandler? OnBackSelected;

        private StandardColor selectedColor;
        private readonly string displayName;

        public PlayerColorMenu(string characterName)
        {
            displayName = characterName;
            selectedColor = StandardColor.Red;

            Options = new[]
            {
                new Option("Color", "Red", "Dark Red", "Yellow", "Dark Yellow", "Green", "Dark Green", "Cyan", "Dark Cyan", "Blue", "Dark Blue", "Magenta", "Dark Magenta")
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
            Screen.WriteLine("Select a color for your name and HUD:\n");
            Screen.ForegroundColor = selectedColor;
            Screen.WriteLine(displayName);
            Screen.WriteLine();
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
                    },
                    Cycle = true
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
            Screen.WriteLine("Select a class for your character:\n");
            Screen.ForegroundColor = StandardColor.DarkGray;
            Screen.WriteLine(selectedClass.Description + "\n");
            Screen.ResetColor();

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
                    },
                    Cycle = true
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
            Screen.WriteLine("Select a race for your character:\n");
            Screen.ForegroundColor = StandardColor.DarkGray;
            Screen.WriteLine(selectedRace.Description + "\n");
            Screen.ResetColor();

			selectedRace.Bonuses?.Display();
			selectedRace.AttributeRolls?.Display();
        }
    }
}