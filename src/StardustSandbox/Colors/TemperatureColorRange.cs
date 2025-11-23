using Microsoft.Xna.Framework;

namespace StardustSandbox.Colors
{
    internal readonly struct TemperatureColorRange(double minimumTemperature, double maximumTemperature, Color color)
    {
        internal readonly double MinimumTemperature => minimumTemperature;
        internal readonly double MaximumTemperature => maximumTemperature;
        internal readonly Color Color => color;
    }
}
