using System;

namespace PixelDust.Game.Mathematics
{
    public static class PTemperatureMath
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
