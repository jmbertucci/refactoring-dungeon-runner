using System;
using System.Collections.Generic;

namespace refactoring_dungeon_runner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();
            int playerHealth = 100;
            int gold = 0;
            int x = 0;
            int y = 0;
            List<string> inventory = new List<string>();
            bool gameOver = false;
            string playerName = "";

            // Possible phrases when entering a new room (includes the original line).
            string[] roomEntryPhrases = new[]
            {
                "You step cautiously into the next chamber...",
                "You tiptoe in—no time to dilly dally.",
                "You stride inside—no shilly-shally.",
                "You sneak through the doorway, hush-hush.",
                "You slide into the room, quick as a flick.",
                "You saunter in without a fuss or muss.",
                "You slink in, slick as a shadow.",
                "You meander in—clickety-clack go your boots.",
                "You hop in with a skip and a step.",
                "You shuffle in—steady and ready.",
                "You barrel in—whoops-a-daisy!",
                "You creep in, sneaky-beaky.",
                "You glide in like a whisper in the dark.",
                "You poke your head in—peeky-peek.",
                "You slip inside—hocus pocus.",
                "You enter—no dilly dally, tally-ho!"
            };

            Console.WriteLine("Welcome to Dungeon Runner!");
            Console.Write("Enter your name: ");
            playerName = Console.ReadLine();
            Console.WriteLine($"Greetings, {playerName}! You awaken in a dark dungeon...");

            while (!gameOver)
            {
                Console.WriteLine();
                Console.WriteLine($"Location: ({x},{y}) | Health: {playerHealth} | Gold: {gold}");
                Console.Write("Move (N/S/E/W), type HELP, or Q to quit: ");
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) continue;

                if (input.ToUpper() == "HELP" || input.ToUpper() == "H")
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
                    Console.WriteLine(" - In combat: A to Attack, R to Run");
                    Console.WriteLine(" - At fountains: D to Drink, I to Ignore");
                    continue;
                }

                if (input.ToUpper() == "Q")
                {
                    Console.WriteLine("You decide to end your journey...");
                    break;
                }

                if (input.ToUpper() == "N") y++;
                else if (input.ToUpper() == "S") y--;
                else if (input.ToUpper() == "E") x++;
                else if (input.ToUpper() == "W") x--;
                else
                {
                    Console.WriteLine("You stumble aimlessly.");
                    continue;
                }

                Console.WriteLine(roomEntryPhrases[random.Next(0, roomEntryPhrases.Length)]);

                int encounterType = random.Next(0, 4);
                if (encounterType == 0)
                {
                    Console.WriteLine("A monster appears!");
                    int monsterHealth = random.Next(20, 60);
                    string monsterName = "";
                    int monsterType = random.Next(0, 3);
                    if (monsterType == 0) monsterName = "Goblin";
                    else if (monsterType == 1) monsterName = "Skeleton";
                    else monsterName = "Slime";

                    Console.WriteLine($"It’s a {monsterName} with {monsterHealth} HP!");

                    bool fightOver = false;
                    while (!fightOver)
                    {
                        Console.Write("(A)ttack or (R)un: ");
                        var choice = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(choice)) continue;

                        if (choice.ToUpper() == "A")
                        {
                            int dmg = random.Next(5, 20);
                            monsterHealth -= dmg;
                            Console.WriteLine($"You hit the {monsterName} for {dmg} damage!");

                            if (monsterHealth <= 0)
                            {
                                Console.WriteLine($"The {monsterName} collapses!");
                                int loot = random.Next(5, 15);
                                gold += loot;
                                Console.WriteLine($"You find {loot} gold coins!");
                                if (random.Next(0, 2) == 0)
                                {
                                    string item = "Potion";
                                    Console.WriteLine($"You also found a {item}!");
                                    inventory.Add(item);
                                }
                                fightOver = true;
                            }
                            else
                            {
                                int mdmg = random.Next(3, 15);
                                playerHealth -= mdmg;
                                Console.WriteLine($"The {monsterName} hits you for {mdmg}!");
                                if (playerHealth <= 0)
                                {
                                    Console.WriteLine("You have been defeated...");
                                    gameOver = true;
                                    break;
                                }
                            }
                        }
                        else if (choice.ToUpper() == "R")
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
                    gold += goldFound;
                    Console.WriteLine($"You collect {goldFound} gold coins!");
                    if (random.Next(0, 3) == 0)
                    {
                        string foundItem = "Elixir";
                        Console.WriteLine($"You found a {foundItem}!");
                        inventory.Add(foundItem);
                    }
                }
                else if (encounterType == 2)
                {
                    Console.WriteLine("You discover an ancient fountain bubbling with strange liquid.");
                    Console.Write("(D)rink or (I)gnore? ");
                    var drinkChoice = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(drinkChoice) && drinkChoice.ToUpper() == "D")
                    {
                        int effect = random.Next(0, 2);
                        if (effect == 0)
                        {
                            int heal = random.Next(10, 30);
                            playerHealth += heal;
                            Console.WriteLine($"You feel refreshed! (+{heal} HP)");
                        }
                        else
                        {
                            int dmg = random.Next(10, 25);
                            playerHealth -= dmg;
                            Console.WriteLine($"The liquid burns! (-{dmg} HP)");
                            if (playerHealth <= 0)
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
                    playerHealth += heal;
                    Console.WriteLine($"You recover {heal} HP.");
                }

                if (playerHealth > 100) playerHealth = 100;

                Console.WriteLine();
                Console.WriteLine("Your inventory: " + (inventory.Count == 0 ? "(empty)" : string.Join(", ", inventory)));

                if (gold >= 100)
                {
                    Console.WriteLine("You’ve gathered enough gold to escape the dungeon!");
                    gameOver = true;
                }

                if (playerHealth <= 0)
                {
                    Console.WriteLine("You’ve perished in the depths of the dungeon.");
                    gameOver = true;
                }
            }

            Console.WriteLine();
            Console.WriteLine("=== Game Over ===");
            Console.WriteLine($"{playerName} finished with {gold} gold and {playerHealth} HP.");
            Console.WriteLine("Thanks for playing Dungeon Runner!");
        }
    }
}
