using System;
using refactoring_dungeon_runner;

namespace refactoring_dungeonrunner
{
    public static class EncounterFactory
    {
        public static IEncounter CreateRandom(Player player, Random random)
        {
            int encounterType = random.Next(0, 4);
            return encounterType switch
            {
                0 => new CombatEncounter(player, random),
                1 => new ChestEncounter(player, random),
                2 => new FountainEncounter(player, random),
                _ => new QuietRoomEncounter(player, random),
            };
        }
    }
}