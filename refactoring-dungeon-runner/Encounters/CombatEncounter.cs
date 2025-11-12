using System;
using System.Collections.Generic;
using refactoring_dungeon_runner;

namespace refactoring_dungeonrunner
{
    public sealed class CombatEncounter : IEncounter
    {
        private readonly Player _player;
        private readonly Random _random;

        private string _monsterName = string.Empty;
        private int _monsterHealth;

        public GameMode Mode => GameMode.Combat;
        public bool IsComplete { get; private set; }
        public bool EndGame { get; private set; }

        public CombatEncounter(Player player, Random random)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _random = random ?? throw new ArgumentNullException(nameof(random));
        }

        public IEnumerable<string> OnEnter()
        {
            yield return "A monster appears!";
            _monsterHealth = _random.Next(20, 60);
            _monsterName = _random.Next(0, 3) switch
            {
                0 => "Goblin",
                1 => "Skeleton",
                _ => "Slime"
            };
            yield return $"It's a {_monsterName} with {_monsterHealth} HP!";
        }

        public IEnumerable<string> OnInput(string cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd))
                yield break;

            var c = cmd.Trim().ToUpperInvariant();

            if (c == "Q")
            {
                yield return "You abandon the fight and give up your quest...";
                EndGame = true;
                IsComplete = true;
                yield break;
            }

            if (c == "A")
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

                    IsComplete = true;
                    yield break;
                }

                // Monster turn
                int mdmg = _random.Next(3, 15);
                _player.Damage(mdmg);
                yield return $"The {_monsterName} hits you for {mdmg}!";

                if (_player.IsDead)
                {
                    yield return "You have been defeated...";
                    EndGame = true;
                    IsComplete = true;
                }
                yield break;
            }

            if (c == "R")
            {
                yield return "You run away!";
                IsComplete = true;
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
                EndGame = true;
                IsComplete = true;
            }
        }

        public string GetPrompt() => "(A)ttack, (R)un or (Q)uit: ";
    }
}