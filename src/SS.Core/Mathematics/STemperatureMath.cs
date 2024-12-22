using StardustSandbox.Core.Constants;

namespace StardustSandbox.Core.Mathematics
{
    public static class STemperatureMath
    {
        public static short Clamp(short value)
        {
            return short.Clamp(value, STemperatureConstants.MIN_CELSIUS_VALUE, STemperatureConstants.MAX_CELSIUS_VALUE);
        }
    }
}
