using System;
using System.Collections.Generic;

namespace refactoring_dungeon_runner
{
    internal class Program : DungeonRunner
    {
        static void Main(string[] args)
        {
            var game = new DungeonRunner();
            game.Run();
        }
    }
}
