using System;
using System.Collections.Generic;
using Lilac.Entities;
using Lilac.Entities.Creatures;
using Lilac.Menus;
using Lilac.Rendering;

namespace Lilac;

public sealed class Game
{
	public enum Difficulty
	{
		Easy,
		Normal,
		Hard,
		Count
	}

	private readonly Stack<IMenu> menus = new();
	private bool inGame;

	public Game()
	{
		Singleton = this;
	}

	private IMenu? CurrentMenu => menus.TryPeek(out var menu) ? menu : null;

	public static Game? Singleton { get; private set; }

	public Character Character { get; private set; } = new();
	public Player? Player { get; private set; }
	public List<Creature> Party { get; } = new();

	public Difficulty CurrentDifficulty { get; private set; } = Difficulty.Normal;

	~Game()
	{
		if (Singleton == this)
			Singleton = null;
	}

	private void EndGame()
	{
		Player = null;
		Party.Clear();
		Character = new Character();
	}

	public void Start()
	{
		var mainMenu = new MainMenu();

		mainMenu.OnSettingsSelected += () =>
		{
			var settingsMenu = new SettingsMenu();

			settingsMenu.SetDifficulty(CurrentDifficulty);
			settingsMenu.OnBackSelected += () => menus.Pop();
			settingsMenu.OnDifficultyChanged += difficulty => CurrentDifficulty = difficulty;

			menus.Push(settingsMenu);
		};

		mainMenu.OnPlaySelected += () =>
		{
			var preIntroMenu = new PreIntroMenu();

			preIntroMenu.OnOkaySelected += () =>
			{
				var characterCreationMenu = new CharacterCreationMenu(Character);
				characterCreationMenu.OnCharacterFinished += () =>
				{
					var player = new Player(Character);

					Player = player;
					Party.Add(player);

					menus.Clear();

					// Start game here!
					var gameMenu = new GameMenu();
					menus.Push(gameMenu);
				};

				menus.Push(characterCreationMenu);
				inGame = true;
			};

			menus.Push(preIntroMenu);
		};

		var running = true;
		mainMenu.OnQuitSelected += () => running = false;

		menus.Push(mainMenu);

		while (running)
		{
			Screen.Clear();

			// Render
			CurrentMenu?.RenderTitle();
			CurrentMenu?.RenderOptions();

			// Process
			var key = Screen.ReadKey();

			if (CurrentMenu?.HandleKey(key) ?? false)
				continue;

			// Handle Pause Menu
			if (key.Key != ConsoleKey.Escape)
				continue;

			if (CurrentMenu is PauseMenu)
			{
				menus.Pop();
			}
			else if (inGame)
			{
				var pauseMenu = new PauseMenu();
				pauseMenu.OnResumeSelected += () => menus.Pop();
				pauseMenu.OnQuitToMainMenuSelected += () =>
				{
					menus.Clear();
					inGame = false;
					EndGame();
					menus.Push(mainMenu);
				};

				pauseMenu.OnQuitToDesktopSelected += () => running = false;

				menus.Push(pauseMenu);
			}
		}
	}
}