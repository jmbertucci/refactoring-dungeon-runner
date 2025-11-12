using System;
using System.Collections.Generic;
using refactoring_dungeon_runner;

namespace refactoring_dungeonrunner
{
    public sealed class QuietRoomEncounter : IEncounter
    {
        private readonly Player _player;
        private readonly Random _random;

        public GameMode Mode => GameMode.Exploration;
        public bool IsComplete { get; private set; }
        public bool EndGame => false;

        public QuietRoomEncounter(Player player, Random random)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _random = random ?? throw new ArgumentNullException(nameof(random));
        }

        public IEnumerable<string> OnEnter()
        {
            yield return "The room is eerily quiet... you take a short rest.";
            int heal = _random.Next(5, 15);
            _player.Heal(heal);
            yield return $"You recover {heal} HP.";
            IsComplete = true;
        }

        public IEnumerable<string> OnInput(string cmd)
        {
            // Non-interactive; no input expected.
            yield break;
        }

        public string GetPrompt() => string.Empty;
    }
}