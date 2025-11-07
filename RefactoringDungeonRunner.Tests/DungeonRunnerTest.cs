using refactoring_dungeon_runner;

namespace RefactoringDungeonRunner.Tests
{
    public class DungeonRunnerTests
    {
        [Fact]
        public void Run_WithNameAndQuitInput_DoesNotThrow()
        {
            // Arrange: Provide a player name and then Q to quit immediately.
            var input = new StringReader("Tester\nQ\n");
            Console.SetIn(input);
            // Suppress console output during the test.
            var output = new StringWriter();
            Console.SetOut(output);

            var runner = new DungeonRunner();

            // Act
            var ex = Record.Exception(() => runner.Run());

            // Assert
            Assert.Null(ex);
            var consoleText = output.ToString();
            Assert.Contains("Welcome to Dungeon Runner!", consoleText);
            Assert.Contains("=== Game Over ===", consoleText);
        }
    }
}