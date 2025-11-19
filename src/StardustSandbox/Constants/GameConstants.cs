using System;

namespace StardustSandbox.Constants
{
    internal static class GameConstants
    {
        internal static Version VERSION => new(1, 2, 2, 0);

        internal const string TITLE = "StardustSandbox";
        internal const string AUTHOR = "Davi \"Starciad\" Fernandes";
        internal const ushort YEAR = 2025;

        internal static string GetTitleAndVersionString()
        {
            return string.Concat(TITLE, " - v", VERSION);
        }
    }
}
