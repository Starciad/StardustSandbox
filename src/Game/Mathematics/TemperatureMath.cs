using StardustSandbox.Constants;

namespace StardustSandbox.Mathematics
{
    internal static class TemperatureMath
    {
        internal static float Clamp(float value)
        {
            return float.Clamp(value, TemperatureConstants.MIN_CELSIUS_VALUE, TemperatureConstants.MAX_CELSIUS_VALUE);
        }
    }
}
