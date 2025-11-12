using System;

namespace refactoring_dungeonrunner
{
    public interface IEncounter
    {
        GameMode Mode { get; }
        bool IsComplete { get; }
        bool EndGame { get; }

        // Called when the encounter starts (after entering a room)
        System.Collections.Generic.IEnumerable<string> OnEnter();

        // Called for interactive encounters (Combat, Fountain). Non-interactive ones can ignore input.
        System.Collections.Generic.IEnumerable<string> OnInput(string cmd);

        // Returns the prompt for interactive encounters, or empty string otherwise.
        string GetPrompt();
    }
}