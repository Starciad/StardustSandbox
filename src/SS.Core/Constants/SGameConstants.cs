using System;

namespace StardustSandbox.Core.Constants
{
    public static class SGameConstants
    {
        public const string TITLE = "StardustSandbox";
        public static Version VERSION => new(0, 0, 1, 0);

        public static string GetTitleAndVersionString()
        {
            return string.Concat(TITLE, " - ", VERSION);
        }
    }
}
