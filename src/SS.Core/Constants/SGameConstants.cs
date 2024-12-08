using System;

namespace StardustSandbox.Core.Constants
{
    public static class SGameConstants
    {
        public const string TITLE = "StardustSandbox";
        public const string AUTHOR = "Davi \"Starciad\" Fernandes";
        public const ushort YEAR = 2024;
        public static Version VERSION => new(1, 0, 0, 0);

        public static string GetTitleAndVersionString()
        {
            return string.Concat(TITLE, " - ", VERSION);
        }
    }
}
