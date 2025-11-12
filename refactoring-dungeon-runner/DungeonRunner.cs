using refactoring_dungeon_runner;
using System;

namespace refactoring_dungeonrunner
{
    public class DungeonRunner
    {
        private readonly int _startingHealth;

        public DungeonRunner(int startingHealth = 100)
        {
            if (startingHealth <= 0)
                throw new ArgumentOutOfRangeException(nameof(startingHealth), "Starting health must be positive.");
            _startingHealth = startingHealth;
        }

        public void Run()
        {
            Console.WriteLine("Welcome to Dungeon Runner!");
            Console.Write("Enter your name: ");
            var providedName = Console.ReadLine() ?? string.Empty;
            var player = new Player(providedName, _startingHealth);

            var roomEntryPhrases = Localization.GetRoomEntryPhrases();
            var session = new GameSession(player, roomEntryPhrases);

            Console.WriteLine($"Greetings, {player.Name}! You awaken in a dark dungeon...");

            while (!session.GameOver)
            {
                foreach (var line in session.GetHeaderLines())
                    Console.WriteLine(line);

                Console.Write(session.GetPrompt());
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;

                foreach (var line in session.ProcessInput(input))
                    Console.WriteLine(line);
            }

            Console.WriteLine();
            Console.WriteLine("=== Game Over ===");
            Console.WriteLine($"{player.Name} finished with {player.Gold} gold and {player.Health} HP.");
            Console.WriteLine("Thanks for playing Dungeon Runner!");
        }
    }
}