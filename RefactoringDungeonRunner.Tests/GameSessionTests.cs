using System;
using System.Linq;
using refactoring_dungeon_runner;
using refactoring_dungeonrunner;
using Xunit;

namespace RefactoringDungeonRunner.Tests
{
    public class GameSessionTests
    {
        [Fact]
        public void Exploration_Move_EmitsRoomEntryPhrase_AndInventoryLine()
        {
            var player = new Player("Tester", 100);
            var phrases = new[] { "Phrase A", "Phrase B" };
            var session = new GameSession(player, phrases, forcedFirstEncounterType: 3, random: new Random(1)); // quiet room

            // First header
            var header = session.GetHeaderLines().ToArray();
            Assert.Contains(header, l => l.Contains("Location: (0,0)"));

            // Move north
            var outputs = session.ProcessInput("N").ToArray();
            Assert.True(phrases.Any(p => outputs.Contains(p)));
            Assert.Contains(outputs, l => l.StartsWith("The room is eerily quiet"));
            Assert.Contains(outputs, l => l.StartsWith("Your inventory: "));
            Assert.False(session.GameOver);
        }

        [Fact]

        public void Combat_Quit_EndsGame()
        {
            var player = new Player("Fighter", 100);
            var phrases = new[] { "Enter" };
            var session = new GameSession(player, phrases, forcedFirstEncounterType: 0);

            // Step into combat by moving
            session.ProcessInput("N").ToArray();
            Assert.Equal(GameMode.Combat, session.Mode);

            var outputs = session.ProcessInput("Q").ToArray();
            Assert.Contains(outputs, l => l.Contains("abandon the fight"));
            Assert.True(session.GameOver);
        }

        [Fact]
        public void Fountain_D_Or_I_CompletesAndReturnsToExploration()
        {
            var player = new Player("Drinker", 100);
            var phrases = new[] { "Enter" };
            var session = new GameSession(player, phrases, forcedFirstEncounterType: 2, random: new Random(1));

            // Trigger fountain
            session.ProcessInput("N").ToArray();
            Assert.Equal(GameMode.Fountain, session.Mode);

            var outputs = session.ProcessInput("I").ToArray();
            Assert.Contains(outputs, l => l.Contains("leaving the fountain behind"));
            Assert.Equal(GameMode.Exploration, session.Mode);
        }
    }
}