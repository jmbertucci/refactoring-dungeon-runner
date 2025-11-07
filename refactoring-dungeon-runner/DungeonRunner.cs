using refactoring_dungeon_runner;
using System;
using System.Collections.Generic;

namespace refactoring_dungeonrunner
{
    public class DungeonRunner
    {
        private readonly int _startingHealth;
        private int? _forcedFirstEncounterType;

        public DungeonRunner(int startingHealth = 100, int? forcedFirstEncounterType = null)
        {
            if (startingHealth <= 0)
                throw new ArgumentOutOfRangeException(nameof(startingHealth), "Starting health must be positive.");
            if (forcedFirstEncounterType is < 0 or > 3)
                throw new ArgumentOutOfRangeException(nameof(forcedFirstEncounterType), "Forced encounter type must be 0..3 or null.");

            _startingHealth = startingHealth;
            _forcedFirstEncounterType = forcedFirstEncounterType;
        }

        public void Run()
        {
            var random = new Random();

            Console.WriteLine("Welcome to Dungeon Runner!");
            Console.Write("Enter your name: ");
            var providedName = Console.ReadLine() ?? string.Empty;
            var player = new Player(providedName, _startingHealth);

            int x = 0, y = 0;
            bool gameOver = false;

            var roomEntryPhrases = Localization.GetRoomEntryPhrases();

            Console.WriteLine($"Greetings, {player.Name}! You awaken in a dark dungeon...");

            while (!gameOver)
            {
                Console.WriteLine();
                Console.WriteLine($"Location: ({x},{y}) | Health: {player.Health} | Gold: {player.Gold}");
                Console.Write("Move (N/S/E/W), type HELP, or Q to quit: ");
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) continue;

                var cmd = input.ToUpper();
                if (cmd == "HELP" || cmd == "H")
                {
                    Console.WriteLine("Available commands:");
                    Console.WriteLine(" - N: Move north");
                    Console.WriteLine(" - S: Move south");
                    Console.WriteLine(" - E: Move east");
                    Console.WriteLine(" - W: Move west");
                    Console.WriteLine(" - Q: Quit the game");
                    Console.WriteLine(" - HELP or H: Show this help");
                    Console.WriteLine();
                    Console.WriteLine("Contextual commands:");
                    Console.WriteLine(" - In combat: A to Attack, R to Run, Q to Quit immediately");
                    Console.WriteLine(" - At fountains: D to Drink, I to Ignore, Q to Quit immediately");
                    continue;
                }

                if (cmd == "Q")
                {
                    Console.WriteLine("You decide to end your journey...");
                    break;
                }

                if (cmd == "N") y++;
                else if (cmd == "S") y--;
                else if (cmd == "E") x++;
                else if (cmd == "W") x--;
                else
                {
                    Console.WriteLine("You stumble aimlessly.");
                    continue;
                }

                if (roomEntryPhrases.Length > 0)
                {
                    var phrase = roomEntryPhrases[random.Next(0, roomEntryPhrases.Length)];
                    Console.WriteLine(phrase);
                }

                int encounterType = _forcedFirstEncounterType.HasValue
                    ? _forcedFirstEncounterType.Value
                    : random.Next(0, 4);
                _forcedFirstEncounterType = null; // Only applies to first encounter if set.

                if (encounterType == 0)
                {
                    Console.WriteLine("A monster appears!");
                    int monsterHealth = random.Next(20, 60);
                    string monsterName = random.Next(0, 3) switch
                    {
                        0 => "Goblin",
                        1 => "Skeleton",
                        _ => "Slime"
                    };
                    Console.WriteLine($"It's a {monsterName} with {monsterHealth} HP!");

                    bool fightOver = false;
                    while (!fightOver && !gameOver)
                    {
                        Console.Write("(A)ttack, (R)un or (Q)uit: ");
                        var choice = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(choice)) continue;

                        var c = choice.ToUpper();
                        if (c == "Q")
                        {
                            Console.WriteLine("You abandon the fight and give up your quest...");
                            gameOver = true;
                            fightOver = true;
                            break;
                        }

                        if (c == "A")
                        {
                            int dmg = random.Next(5, 20);
                            monsterHealth -= dmg;
                            Console.WriteLine($"You hit the {monsterName} for {dmg} damage!");

                            if (monsterHealth <= 0)
                            {
                                Console.WriteLine($"The {monsterName} collapses!");
                                int loot = random.Next(5, 15);
                                player.AddGold(loot);
                                Console.WriteLine($"You find {loot} gold coins!");
                                if (random.Next(0, 2) == 0)
                                {
                                    string item = "Potion";
                                    Console.WriteLine($"You also found a {item}!");
                                    player.AddItem(item);
                                }
                                fightOver = true;
                            }
                            else
                            {
                                int mdmg = random.Next(3, 15);
                                player.Damage(mdmg);
                                Console.WriteLine($"The {monsterName} hits you for {mdmg}!");
                                if (player.IsDead)
                                {
                                    Console.WriteLine("You have been defeated...");
                                    gameOver = true;
                                    break;
                                }
                            }
                        }
                        else if (c == "R")
                        {
                            Console.WriteLine("You run away!");
                            fightOver = true;
                        }
                        else
                        {
                            Console.WriteLine("You hesitate and lose your turn!");
                        }
                    }
                }
                else if (encounterType == 1)
                {
                    Console.WriteLine("You find a chest!");
                    int goldFound = random.Next(10, 40);
                    player.AddGold(goldFound);
                    Console.WriteLine($"You collect {goldFound} gold coins!");
                    if (random.Next(0, 3) == 0)
                    {
                        string foundItem = "Elixir";
                        Console.WriteLine($"You found a {foundItem}!");
                        player.AddItem(foundItem);
                    }
                }
                else if (encounterType == 2)
                {
                    Console.WriteLine("You discover an ancient fountain bubbling with strange liquid.");
                    Console.Write("(D)rink, (I)gnore or (Q)uit? ");
                    var drinkChoice = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(drinkChoice)) continue;

                    var dc = drinkChoice.ToUpper();
                    if (dc == "Q")
                    {
                        Console.WriteLine("You decide the dungeon is too perilous and depart...");
                        gameOver = true;
                        continue;
                    }

                    if (dc == "D")
                    {
                        int effect = random.Next(0, 2);
                        if (effect == 0)
                        {
                            int heal = random.Next(10, 30);
                            player.Heal(heal);
                            Console.WriteLine($"You feel refreshed! (+{heal} HP)");
                        }
                        else
                        {
                            int dmg = random.Next(10, 25);
                            player.Damage(dmg);
                            Console.WriteLine($"The liquid burns! (-{dmg} HP)");
                            if (player.IsDead)
                            {
                                Console.WriteLine("You collapse beside the cursed fountain...");
                                gameOver = true;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("You move on, leaving the fountain behind.");
                    }
                }
                else
                {
                    Console.WriteLine("The room is eerily quiet... you take a short rest.");
                    int heal = random.Next(5, 15);
                    player.Heal(heal);
                    Console.WriteLine($"You recover {heal} HP.");
                }

                Console.WriteLine();
                Console.WriteLine("Your inventory: " + (player.Inventory.Count == 0 ? "(empty)" : string.Join(", ", player.Inventory)));

                if (player.Gold >= 100)
                {
                    Console.WriteLine("You've gathered enough gold to escape the dungeon!");
                    gameOver = true;
                }

                if (player.IsDead)
                {
                    Console.WriteLine("You've perished in the depths of the dungeon.");
                    gameOver = true;
                }
            }

            Console.WriteLine();
            Console.WriteLine("=== Game Over ===");
            Console.WriteLine($"{player.Name} finished with {player.Gold} gold and {player.Health} HP.");
            Console.WriteLine("Thanks for playing Dungeon Runner!");
        }
    }
}