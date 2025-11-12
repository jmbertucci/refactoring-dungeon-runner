using System;
using System.Collections.Generic;
using refactoring_dungeon_runner;

namespace refactoring_dungeonrunner
{
    public sealed class FountainEncounter : IEncounter
    {
        private readonly Player _player;
        private readonly Random _random;

        public GameMode Mode => GameMode.Fountain;
        public bool IsComplete { get; private set; }
        public bool EndGame { get; private set; }

        public FountainEncounter(Player player, Random random)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _random = random ?? throw new ArgumentNullException(nameof(random));
        }

        public IEnumerable<string> OnEnter()
        {
            yield return "You discover an ancient fountain bubbling with strange liquid.";
        }

        public IEnumerable<string> OnInput(string cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd))
                yield break;

            var c = cmd.Trim().ToUpperInvariant();

            if (c == "Q")
            {
                yield return "You decide the dungeon is too perilous and depart...";
                EndGame = true;
                IsComplete = true;
                yield break;
            }

            if (c == "D")
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
                        EndGame = true;
                    }
                }

                IsComplete = true;
                yield break;
            }

            // Ignore or any other input
            yield return "You move on, leaving the fountain behind.";
            IsComplete = true;
        }

        public string GetPrompt() => "(D)rink, (I)gnore or (Q)uit? ";
    }
}