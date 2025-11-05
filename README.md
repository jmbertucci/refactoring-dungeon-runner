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
- test
  - test
  - test
  - test
- [ ] todo
- [x] done
- robot

# Refactoring Dungeon Runner Tests

This is the test project for the Refactoring Dungeon Runner. It contains unit tests and integration tests to ensure the quality and correctness of the application.

## Project Structure

- **RefactoringDungeonRunner.Tests.csproj**: The project file for the test project.
- **UnitTests/**: Contains unit tests for individual components.
- **IntegrationTests/**: Contains integration tests for testing interactions between components.

## Running the Tests

### In Visual Studio

- Open the Test Explorer: `Test > Test Explorer`.
- Run all tests: Click the "Run All" button in the Test Explorer.
- Run selected tests: Select the tests you want to run and click the "Run" button.

### Using .NET CLI

- Open a terminal and navigate to the test project folder.
- Run all tests: `dotnet test`.
- Run a specific test: `dotnet test --filter "Namespace.ClassName.MethodName"`.

### Notes

- Ensure the main project builds successfully before running the tests.
- Tests may contain dependencies on external resources (e.g., databases, APIs). Ensure these resources are available and properly configured.
- Review and update tests as needed when refactoring or adding new features.

## Dependencies

This project references the following NuGet packages:

- **xunit**: A popular testing framework for .NET.
- **xunit.runner.visualstudio**: Provides test runner integration for Visual Studio.
- **coverlet.collector**: Collects code coverage data for the tests.

## Configuration

The test project is configured to target .NET 8 and enables nullable reference types. Implicit usings are also enabled to reduce boilerplate code.

A facilitated code review checklist:

- [ ] Are the new features and fixes covered by tests?
- [ ] Are the tests runnable in both Visual Studio and the .NET CLI?
- [ ] Is the test project organized and easy to navigate?
- [ ] Are test method names descriptive and following the naming conventions?
- [ ] Is the code formatting consistent with the main project?
- [ ] Are unnecessary usings and comments removed?

