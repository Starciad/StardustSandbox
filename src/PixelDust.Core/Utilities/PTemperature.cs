using System;

namespace PixelDust.Core.Utilities
{
    internal static class PTemperature
    {
        internal const double MinCelsiusValue = -275f;
        internal const double MaxCelsiusValue = 9725.85f;

        internal const float HeatExchangeRate = 1.5f;
        internal const float EquilibriumThreshold = 0.1f;

        internal static double GetCelsiusInterval(double value)
        {
            return Math.Clamp(value, MinCelsiusValue, MaxCelsiusValue);
        }
    }
}
