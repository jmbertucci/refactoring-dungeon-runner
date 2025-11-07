using System;
using System.Globalization;
using System.Resources;

namespace refactoring_dungeon_runner
{
    public static class Localization
    {
        // Base name = <default namespace>.<folder>.<resx file name without extension>
        private static readonly ResourceManager ResourceManager =
            new ResourceManager("refactoring_dungeon_runner.Resources.Strings", typeof(Localization).Assembly);

        public static string[] GetRoomEntryPhrases()
        {
            var raw = ResourceManager.GetString("RoomEntryPhrases", CultureInfo.CurrentUICulture) ?? string.Empty;
            return raw.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }
    }
}