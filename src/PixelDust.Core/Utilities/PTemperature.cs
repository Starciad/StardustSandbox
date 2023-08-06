using System;

namespace PixelDust.Core.Utilities
{
    internal static class PTemperature
    {
        internal static double MinCelsiusValue => -275f;
        internal static double MaxCelsiusValue => 9725.85f;

        internal static double GetCelsiusInterval(double value)
        {
            return Math.Clamp(value, MinCelsiusValue, MaxCelsiusValue);
        }
    }
}
