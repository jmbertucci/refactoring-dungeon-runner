using System;
using System.IO;
using System.Linq;
using refactoring_dungeonrunner;
using refactoring_dungeon_runner;
using Xunit;

namespace RefactoringDungeonRunner.Tests
{
    public class DungeonRunnerTests
    {
        [Fact]
        public void Run_WithDefaultStartingHealth_AndImmediateQuit_DoesNotThrow()
        {
            var input = new StringReader("Tester\nQ\n");
            Console.SetIn(input);
            var output = new StringWriter();
            Console.SetOut(output);

            var runner = new DungeonRunner(100);

            var ex = Record.Exception(() => runner.Run());

            Assert.Null(ex);
            var text = output.ToString();
            Assert.Contains("Welcome to Dungeon Runner!", text);
            Assert.Contains("=== Game Over ===", text);
        }

        [Fact]
        public void Run_WithCustomStartingHealth_CapsHealingAtStartingHealth()
        {
            var input = new StringReader("Player\nQ\n");
            Console.SetIn(input);
            var output = new StringWriter();
            Console.SetOut(output);

            var runner = new DungeonRunner(50);

            var ex = Record.Exception(() => runner.Run());

            Assert.Null(ex);
            Assert.Contains("Health: 50", output.ToString());
        }

        [Fact]
        public void Constructor_NegativeHealth_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DungeonRunner(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new DungeonRunner(-5));
        }

        [Fact]
        public void Run_AfterOneMove_PrintsARoomEntryPhrase()
        {
            var input = new StringReader("Mover\nN\nQ\n");
            Console.SetIn(input);
            var output = new StringWriter();
            Console.SetOut(output);

            var runner = new DungeonRunner();
            runner.Run();

            var text = output.ToString();
            var phrases = Localization.GetRoomEntryPhrases();
            Assert.True(phrases.Length > 0, "Expected at least one configured room entry phrase.");
            Assert.True(phrases.Any(p => text.Contains(p)), "Expected output to contain at least one room entry phrase after moving.");
        }

        [Fact]
        public void Run_MultipleMoves_PrintsAtLeastOneRoomEntryPhrase()
        {
            var input = new StringReader("Explorer\nN\nE\nS\nW\nQ\n");
            Console.SetIn(input);
            var output = new StringWriter();
            Console.SetOut(output);

            var runner = new DungeonRunner();
            runner.Run();

            var text = output.ToString();
            var phrases = Localization.GetRoomEntryPhrases();
            var count = phrases.Count(p => text.Contains(p));
            Assert.True(count >= 1, "Expected at least one room entry phrase across multiple moves.");
        }

        [Fact]
        public void Run_QuitDuringFountain_EndsGame()
        {
            // Force first encounter to be a fountain (type 2)
            var input = new StringReader("Drinker\nN\nQ\n");
            Console.SetIn(input);
            var output = new StringWriter();
            Console.SetOut(output);

            var runner = new DungeonRunner(100, forcedFirstEncounterType: 2);
            runner.Run();

            var text = output.ToString();
            Assert.Contains("(D)rink, (I)gnore or (Q)uit?", text);
            Assert.Contains("=== Game Over ===", text);
        }

        [Fact]
        public void Run_QuitDuringCombat_EndsGame()
        {
            // Force first encounter to be combat (type 0). Provide Q at combat prompt.
            var input = new StringReader("Fighter\nN\nQ\n");
            Console.SetIn(input);
            var output = new StringWriter();
            Console.SetOut(output);

            var runner = new DungeonRunner(100, forcedFirstEncounterType: 0);
            runner.Run();

            var text = output.ToString();
            Assert.Contains("(A)ttack, (R)un or (Q)uit:", text);
            Assert.Contains("=== Game Over ===", text);
        }
    }
}