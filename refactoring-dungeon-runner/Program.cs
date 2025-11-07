using System;
using System.Collections.Generic;

namespace refactoring_dungeonrunner
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
