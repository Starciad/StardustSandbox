using System;

namespace PixelDust.Game.Utilities
{
    public static class PTemperature
    {
        public const short MinCelsiusValue = -275;
        public const short MaxCelsiusValue = 9725;

        public const short EquilibriumThreshold = 1;

        public static short Clamp(int value)
        {
            return (short)Math.Clamp(value, MinCelsiusValue, MaxCelsiusValue);
        }
    }
}
