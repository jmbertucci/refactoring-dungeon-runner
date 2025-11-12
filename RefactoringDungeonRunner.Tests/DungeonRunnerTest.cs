using System;
using System.IO;
using System.Linq;
using refactoring_dungeonrunner;
using refactoring_dungeon_runner; // For Localization
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
        public void Run_WithCustomStartingHealth_PrintsStartingHealthInHeader()
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
        public void Run_AfterOneMove_PrintsARoomEntryPhrase()
        {
            // Name, one move north, then quit
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
        public void Constructor_NonPositiveHealth_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DungeonRunner(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new DungeonRunner(-5));
        }
    }
}