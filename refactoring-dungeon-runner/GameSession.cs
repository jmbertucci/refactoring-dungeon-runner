using System;
using System.Collections.Generic;
using refactoring_dungeon_runner; // Player

namespace refactoring_dungeonrunner
{
    public enum GameMode
    {
        Exploration,
        Combat,
        Fountain,
        GameOver
    }

    public sealed class GameSession
    {
        private readonly Player _player;
        private readonly Random _random;
        private readonly string[] _roomEntryPhrases;

        private int _x;
        private int _y;

        private IEncounter? _currentEncounter;

        public GameMode Mode { get; private set; } = GameMode.Exploration;
        public bool GameOver => Mode == GameMode.GameOver;

        public GameSession(Player player, string[] roomEntryPhrases, Random? random = null)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _roomEntryPhrases = roomEntryPhrases ?? Array.Empty<string>();
            _random = random ?? new Random();
        }

        public IEnumerable<string> GetHeaderLines()
        {
            if (Mode == GameMode.Exploration)
            {
                yield return string.Empty;
                yield return $"Location: ({_x},{_y}) | Health: {_player.Health} | Gold: {_player.Gold}";
            }
        }

        public string GetPrompt()
        {
            return Mode switch
            {
                GameMode.Exploration => "Move (N/S/E/W), type HELP, or Q to quit: ",
                _ => _currentEncounter?.GetPrompt() ?? string.Empty
            };
        }

        public IEnumerable<string> ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                yield break;

            var cmd = input.Trim().ToUpperInvariant();

            switch (Mode)
            {
                case GameMode.Exploration:
                    foreach (var line in ProcessExploration(cmd))
                        yield return line;
                    break;

                case GameMode.Combat:
                case GameMode.Fountain:
                    if (_currentEncounter is null)
                        yield break;

                    foreach (var line in _currentEncounter.OnInput(cmd))
                        yield return line;

                    if (_currentEncounter.IsComplete)
                    {
                        if (_currentEncounter.EndGame)
                        {
                            Mode = GameMode.GameOver;
                            yield break;
                        }

                        // Return to exploration after encounter completes.
                        Mode = GameMode.Exploration;

                        foreach (var post in PostStep())
                            yield return post;

                        // Clear encounter
                        _currentEncounter = null;
                    }
                    break;
            }
        }

        private IEnumerable<string> ProcessExploration(string cmd)
        {
            if (cmd is "HELP" or "H")
            {
                yield return "Available commands:";
                yield return " - N: Move north";
                yield return " - S: Move south";
                yield return " - E: Move east";
                yield return " - W: Move west";
                yield return " - Q: Quit the game";
                yield return " - HELP or H: Show this help";
                yield return string.Empty;
                yield return "Contextual commands:";
                yield return " - In combat: A to Attack, R to Run, Q to Quit immediately";
                yield return " - At fountains: D to Drink, I to Ignore, Q to Quit immediately";
                yield break;
            }

            if (cmd == "Q")
            {
                yield return "You decide to end your journey...";
                Mode = GameMode.GameOver;
                yield break;
            }

            if (cmd == "N") _y++;
            else if (cmd == "S") _y--;
            else if (cmd == "E") _x++;
            else if (cmd == "W") _x--;
            else
            {
                yield return "You stumble aimlessly.";
                yield break;
            }

            // Room entry phrase
            if (_roomEntryPhrases.Length > 0)
            {
                var phrase = _roomEntryPhrases[_random.Next(0, _roomEntryPhrases.Length)];
                yield return phrase;
            }

            // Create and enter a random encounter
            _currentEncounter = EncounterFactory.CreateRandom(_player, _random);

            foreach (var line in _currentEncounter.OnEnter())
                yield return line;

            // If non-interactive, immediately process PostStep and clear
            if (_currentEncounter.IsComplete)
            {
                if (_currentEncounter.EndGame)
                {
                    Mode = GameMode.GameOver;
                    yield break;
                }

                foreach (var post in PostStep())
                    yield return post;

                _currentEncounter = null;
                yield break;
            }

            // Interactive encounters switch the mode
            Mode = _currentEncounter.Mode;
        }

        private IEnumerable<string> PostStep()
        {
            // Inventory line
            yield return string.Empty;
            yield return "Your inventory: " + (_player.Inventory.Count == 0 ? "(empty)" : string.Join(", ", _player.Inventory));

            // Win/Lose checks
            if (_player.Gold >= 100)
            {
                yield return "You've gathered enough gold to escape the dungeon!";
                Mode = GameMode.GameOver;
                yield break;
            }

            if (_player.IsDead)
            {
                yield return "You've perished in the depths of the dungeon.";
                Mode = GameMode.GameOver;
            }
        }
    }
}