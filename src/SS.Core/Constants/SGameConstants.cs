using System;

namespace StardustSandbox.Core.Constants
{
    public static class SGameConstants
    {
        public static Version VERSION => new(1, 2, 0, 0);

        public const string TITLE = "StardustSandbox";
        public const string AUTHOR = "Davi \"Starciad\" Fernandes";
        public const ushort YEAR = 2025;

        public static string GetTitleAndVersionString()
        {
            return string.Concat(TITLE, " - v", VERSION);
        }
    }
}
