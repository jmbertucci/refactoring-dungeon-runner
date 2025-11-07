using System;
using System.IO;
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
    }
}