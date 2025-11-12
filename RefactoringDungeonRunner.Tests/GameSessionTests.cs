using System;
using System.Collections.Generic;
using System.Linq;
using refactoring_dungeon_runner;
using refactoring_dungeonrunner;
using Xunit;

namespace RefactoringDungeonRunner.Tests
{
    // Deterministic Random for tests: returns queued numbers for Next(min,max)
    internal sealed class DeterministicRandom : Random
    {
        private readonly Queue<int> _values;

        public DeterministicRandom(IEnumerable<int> values)
        {
            _values = new Queue<int>(values);
        }

        public override int Next(int minValue, int maxValue)
        {
            if (_values.Count == 0)
            {
                // Fallback to middle of range if exhausted
                return minValue + ((maxValue - minValue) / 2);
            }

            var raw = _values.Dequeue();
            // Map provided raw to the requested range
            var span = maxValue - minValue;
            // Ensure non-negative modulo
            var mapped = minValue + (Math.Abs(raw) % span);
            return mapped;
        }
    }

    public class GameSessionTests
    {
        [Fact]
        public void Exploration_Move_ToQuietRoom_EmitsRoomEntryPhrase_AndInventory()
        {
            // Sequence of Random.Next calls in ProcessExploration:
            // 1) phraseIndex = Next(0, phrases.Length) -> 0
            // 2) encounterType = Next(0,4) -> 3 (quiet room)
            // 3) heal = Next(5,15) -> pick 10
            var rnd = new DeterministicRandom(new[] { 0, 3, 10 });

            var player = new Player("Tester", 100);
            var phrases = new[] { "Phrase A", "Phrase B" };
            var session = new GameSession(player, phrases, rnd);

            // Move north to trigger processing
            var outputs = session.ProcessInput("N").ToArray();

            Assert.Contains("Phrase A", outputs);
            Assert.Contains(outputs, l => l.StartsWith("The room is eerily quiet"));
            Assert.Contains(outputs, l => l.StartsWith("You recover "));
            Assert.Contains(outputs, l => l.StartsWith("Your inventory: "));
            Assert.False(session.GameOver);
        }

        [Fact]
        public void Combat_Quit_EndsGame()
        {
            // Sequence:
            // phraseIndex -> 0
            // encounterType -> 0 (combat)
            // monsterHealth -> Next(20,60) -> map 25
            // monsterName index -> Next(0,3) -> 1 (Skeleton)
            var rnd = new DeterministicRandom(new[] { 0, 0, 25, 1 });

            var player = new Player("Fighter", 100);
            var phrases = new[] { "Entry" };
            var session = new GameSession(player, phrases, rnd);

            // Enter combat
            session.ProcessInput("N").ToArray();
            Assert.Equal(GameMode.Combat, session.Mode);

            var out2 = session.ProcessInput("Q").ToArray();
            Assert.Contains(out2, l => l.Contains("abandon the fight"));
            Assert.True(session.GameOver);
        }

        [Fact]
        public void Fountain_Quit_EndsGame()
        {
            // Sequence:
            // phraseIndex -> 0
            // encounterType -> 2 (fountain)
            var rnd = new DeterministicRandom(new[] { 0, 2 });

            var player = new Player("Drinker", 100);
            var phrases = new[] { "Entry" };
            var session = new GameSession(player, phrases, rnd);

            // Enter fountain
            var out1 = session.ProcessInput("N").ToArray();
            Assert.Equal(GameMode.Fountain, session.Mode);

            var out2 = session.ProcessInput("Q").ToArray();
            Assert.Contains(out2, l => l.Contains("too perilous"));
            Assert.True(session.GameOver);
        }
    }
}