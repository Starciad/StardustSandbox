using StardustSandbox.Constants;

using System;

namespace StardustSandbox.Mathematics
{
    internal static class TemperatureMath
    {
        internal static short Clamp(int value)
        {
            return Clamp(Convert.ToInt16(value));
        }

        internal static short Clamp(short value)
        {
            return short.Clamp(value, TemperatureConstants.MIN_CELSIUS_VALUE, TemperatureConstants.MAX_CELSIUS_VALUE);
        }
    }
}
