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
        private int? _forcedFirstEncounterType;

        private int _x;
        private int _y;

        // Combat state
        private string _monsterName = string.Empty;
        private int _monsterHealth;

        public GameMode Mode { get; private set; } = GameMode.Exploration;

        public bool GameOver => Mode == GameMode.GameOver;

        public GameSession(Player player, string[] roomEntryPhrases, int? forcedFirstEncounterType = null, Random? random = null)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _roomEntryPhrases = roomEntryPhrases ?? Array.Empty<string>();
            _forcedFirstEncounterType = forcedFirstEncounterType;
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

        public string GetPrompt() =>
            Mode switch
            {
                GameMode.Exploration => "Move (N/S/E/W), type HELP, or Q to quit: ",
                GameMode.Combat => "(A)ttack, (R)un or (Q)uit: ",
                GameMode.Fountain => "(D)rink, (I)gnore or (Q)uit? ",
                _ => string.Empty
            };

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
                    foreach (var line in ProcessCombat(cmd))
                        yield return line;
                    break;

                case GameMode.Fountain:
                    foreach (var line in ProcessFountain(cmd))
                        yield return line;
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

            // Determine encounter
            int encounterType = _forcedFirstEncounterType ?? _random.Next(0, 4);
            _forcedFirstEncounterType = null;

            if (encounterType == 0)
            {
                // Combat setup
                yield return "A monster appears!";
                _monsterHealth = _random.Next(20, 60);
                _monsterName = _random.Next(0, 3) switch
                {
                    0 => "Goblin",
                    1 => "Skeleton",
                    _ => "Slime"
                };
                yield return $"It's a {_monsterName} with {_monsterHealth} HP!";
                Mode = GameMode.Combat;
                yield break;
            }

            if (encounterType == 1)
            {
                yield return "You find a chest!";
                int goldFound = _random.Next(10, 40);
                _player.AddGold(goldFound);
                yield return $"You collect {goldFound} gold coins!";
                if (_random.Next(0, 3) == 0)
                {
                    string foundItem = "Elixir";
                    _player.AddItem(foundItem);
                    yield return $"You found a {foundItem}!";
                }
                foreach (var post in PostStep())
                    yield return post;
                yield break;
            }

            if (encounterType == 2)
            {
                yield return "You discover an ancient fountain bubbling with strange liquid.";
                Mode = GameMode.Fountain;
                yield break;
            }

            // Quiet room
            yield return "The room is eerily quiet... you take a short rest.";
            int heal = _random.Next(5, 15);
            _player.Heal(heal);
            yield return $"You recover {heal} HP.";

            foreach (var post in PostStep())
                yield return post;
        }

        private IEnumerable<string> ProcessCombat(string cmd)
        {
            if (cmd == "Q")
            {
                yield return "You abandon the fight and give up your quest...";
                Mode = GameMode.GameOver;
                yield break;
            }

            if (cmd == "A")
            {
                int dmg = _random.Next(5, 20);
                _monsterHealth -= dmg;
                yield return $"You hit the {_monsterName} for {dmg} damage!";

                if (_monsterHealth <= 0)
                {
                    yield return $"The {_monsterName} collapses!";
                    int loot = _random.Next(5, 15);
                    _player.AddGold(loot);
                    yield return $"You find {loot} gold coins!";
                    if (_random.Next(0, 2) == 0)
                    {
                        string item = "Potion";
                        _player.AddItem(item);
                        yield return $"You also found a {item}!";
                    }
                    Mode = GameMode.Exploration;
                    foreach (var post in PostStep())
                        yield return post;
                    yield break;
                }

                // Monster turn
                int mdmg = _random.Next(3, 15);
                _player.Damage(mdmg);
                yield return $"The {_monsterName} hits you for {mdmg}!";

                if (_player.IsDead)
                {
                    yield return "You have been defeated...";
                    Mode = GameMode.GameOver;
                }
                yield break;
            }

            if (cmd == "R")
            {
                yield return "You run away!";
                Mode = GameMode.Exploration;
                foreach (var post in PostStep())
                    yield return post;
                yield break;
            }

            // Invalid input in combat -> lose turn
            yield return "You hesitate and lose your turn!";

            // Monster turn
            int md = _random.Next(3, 15);
            _player.Damage(md);
            yield return $"The {_monsterName} hits you for {md}!";
            if (_player.IsDead)
            {
                yield return "You have been defeated...";
                Mode = GameMode.GameOver;
            }
        }

        private IEnumerable<string> ProcessFountain(string cmd)
        {
            if (cmd == "Q")
            {
                yield return "You decide the dungeon is too perilous and depart...";
                Mode = GameMode.GameOver;
                yield break;
            }

            if (cmd == "D")
            {
                int effect = _random.Next(0, 2);
                if (effect == 0)
                {
                    int heal = _random.Next(10, 30);
                    _player.Heal(heal);
                    yield return $"You feel refreshed! (+{heal} HP)";
                }
                else
                {
                    int dmg = _random.Next(10, 25);
                    _player.Damage(dmg);
                    yield return $"The liquid burns! (-{dmg} HP)";
                    if (_player.IsDead)
                    {
                        yield return "You collapse beside the cursed fountain...";
                        Mode = GameMode.GameOver;
                        yield break;
                    }
                }
                Mode = GameMode.Exploration;
                foreach (var post in PostStep())
                    yield return post;
                yield break;
            }

            // Ignore or any other input
            yield return "You move on, leaving the fountain behind.";
            Mode = GameMode.Exploration;
            foreach (var post in PostStep())
                yield return post;
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