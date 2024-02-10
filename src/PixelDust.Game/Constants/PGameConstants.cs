using System;

namespace PixelDust.Game.Constants
{
    public static class PGameConstants
    {
        public const string TITLE = "PixelDust";
        public static Version VERSION => new(0, 0, 1, 0);

        public static string GetTitleAndVersionString()
        {
            return string.Concat(TITLE, " - ", VERSION);
        }
    }
}
