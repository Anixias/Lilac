using System;
using System.Collections.Generic;
using Lilac.Combat;
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

                        var magicQuizMenu = new MagicQuizMenu(character);
                        magicQuizMenu.OnQuizComplete += () =>
                        {
                            OnCharacterFinished?.Invoke();
                        };

                        currentMenu = magicQuizMenu;
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
            Console.WriteLine("What is your name?");
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

    private sealed class MagicQuizMenu : Menu
    {
        public event EventHandler? OnQuizComplete;

        private sealed class Question
        {
            public string Prompt { get; init; } = "";
            public Option[] Options { get; init; } = Array.Empty<Option>();
        }
        
        private int energyScore;
        private int synthesisScore;
        private int spiritScore;
        private int clockwiseScore;
        private int counterClockwiseScore;
        private int questionIndex;
        private readonly Question[] questions;
        private readonly Character character;

        public MagicQuizMenu(Character character)
        {
            this.character = character;
            
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
                },
            };

            Start();
        }
        
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
            const int offset = 3;
            character.Affinities.Add(DamageType.Fire, energyScore + clockwiseScore - offset);
            character.Affinities.Add(DamageType.Electricity, energyScore + counterClockwiseScore - offset);
            character.Affinities.Add(DamageType.Water, synthesisScore + clockwiseScore - offset);
            character.Affinities.Add(DamageType.Earth, synthesisScore + counterClockwiseScore - offset);
            character.Affinities.Add(DamageType.Shadow, spiritScore + clockwiseScore - offset);
            character.Affinities.Add(DamageType.Air, spiritScore + counterClockwiseScore - offset);
            OnQuizComplete?.Invoke();
        }
    }
}