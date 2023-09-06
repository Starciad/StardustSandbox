using System;

namespace PixelDust.Core.Utilities
{
    internal static class PTemperature
    {
        internal const short MinCelsiusValue = -275;
        internal const short MaxCelsiusValue = 9725;

        internal const short EquilibriumThreshold = 1;

        internal static short Clamp(int value)
        {
            return (short)Math.Clamp(value, MinCelsiusValue, MaxCelsiusValue);
        }
    }
}
