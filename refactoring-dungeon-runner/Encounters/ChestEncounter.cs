using System;
using System.Collections.Generic;
using refactoring_dungeon_runner;

namespace refactoring_dungeonrunner
{
    public sealed class ChestEncounter : IEncounter
    {
        private readonly Player _player;
        private readonly Random _random;

        public GameMode Mode => GameMode.Exploration;
        public bool IsComplete { get; private set; }
        public bool EndGame => false;

        public ChestEncounter(Player player, Random random)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _random = random ?? throw new ArgumentNullException(nameof(random));
        }

        public IEnumerable<string> OnEnter()
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
            IsComplete = true;
        }

        public IEnumerable<string> OnInput(string cmd)
        {
            // Non-interactive; no further input expected.
            yield break;
        }

        public string GetPrompt() => string.Empty;
    }
}