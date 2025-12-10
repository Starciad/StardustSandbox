using Microsoft.Xna.Framework;

namespace StardustSandbox.Colors
{
    internal readonly struct TemperatureColorRange(float minimumTemperature, float maximumTemperature, Color color)
    {
        internal readonly float MinimumTemperature => minimumTemperature;
        internal readonly float MaximumTemperature => maximumTemperature;
        internal readonly Color Color => color;
    }
}
