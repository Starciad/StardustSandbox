using StardustSandbox.Constants;

namespace StardustSandbox.Mathematics
{
    internal static class TemperatureMath
    {
        internal static double Clamp(double value)
        {
            return double.Clamp(value, TemperatureConstants.MIN_CELSIUS_VALUE, TemperatureConstants.MAX_CELSIUS_VALUE);
        }
    }
}
