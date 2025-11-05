# Refactoring Dungeon Runner

A tiny C#/.NET 8 console dungeon crawler designed for refactoring practice. Move through a grid of rooms, fight monsters, collect gold, and try to escape with your life.

## Tech Stack
- .NET 8
- C# 12
- Console app (single-project)
- Entry point: `refactoring-dungeon-runner/Program.cs`

## How to Play
- Goal: Gather 100+ gold to escape the dungeon.
- You start with 100 HP at position (0,0).

Core commands (at the main prompt):
- N, S, E, W: Move north/south/east/west
- HELP or H: Show help
- Q: Quit

Contextual commands:
- In combat: A = Attack, R = Run
- At fountains: D = Drink, I = Ignore

Encounters:
- Monsters: Fight or run. Monsters can drop gold and sometimes items (e.g., Potion).
- Chests: Random gold, sometimes an item (e.g., Elixir).
- Fountains: Can heal or harm.
- Quiet rooms: You rest and regain some health.

Win/Lose:
- Win: Reach 100+ gold.
- Lose: HP drops to 0.

## Run in Visual Studio 2022
Prerequisites:
- Visual Studio 2022 (17.8 or later) with .NET 8 SDK installed.

Steps:
1. Open the repository in VS: __File > Open > Folder__ (or __Open a project or solution__) and select the repo root.
2. In Solution Explorer, right-click the project and choose __Set as Startup Project__.
3. Build: __Build__.
4. Run: __Start Debugging__ (F5) or __Start Without Debugging__ (Ctrl+F5).

## Run with .NET CLI
Prerequisite: .NET 8 SDK



# Notes about Mob Programming

- Move string literalls into a configuration file
  - Can help with localization, easier to change text without recompiling

