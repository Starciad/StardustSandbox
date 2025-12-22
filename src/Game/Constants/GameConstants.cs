using System;

namespace StardustSandbox.Constants
{
    internal static class GameConstants
    {
        internal static Version VERSION => new("2.0.0.0");

        internal const string ID = "stardust_sandbox";
        internal const string TITLE = "Stardust Sandbox";
        internal const string AUTHOR = "Davi \"Starciad\" Fernandes";
        internal const ushort YEAR = 2025;

        internal static string GetTitleAndVersionString()
        {
            return string.Concat(TITLE, " - v", VERSION);
        }
    }
}
