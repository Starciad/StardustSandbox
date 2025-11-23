using Microsoft.Xna.Framework;

using StardustSandbox.Colors;
using StardustSandbox.Colors.Palettes;
using StardustSandbox.Extensions;

namespace StardustSandbox.Constants
{
    internal static class TemperatureConstants
    {
        // Constants for simulation (can be tuned for realism)
        internal const double THERMAL_CONDUCTIVITY = 0.5; // k, arbitrary unit
        internal const double AREA = 1.0; // contact area, arbitrary unit
        internal const double DISTANCE = 1.0; // distance between centers, arbitrary unit

        internal const double MIN_CELSIUS_VALUE = -275.0;
        internal const double MAX_CELSIUS_VALUE = 9725.0;
        internal const double EQUILIBRIUM_THRESHOLD = 1.0;
        internal const double COLOR_HEAT_FACTOR = 0.45;

        internal static readonly TemperatureColorRange[] TEMPERATURE_COLOR_RANGES =
        [
            new(-275.0, -200.0, AAP64ColorPalette.DarkTeal),
            new(-199.0, -150.0, AAP64ColorPalette.NavyBlue),
            new(-149.0, -100.0, AAP64ColorPalette.RoyalBlue),
            new(-99.0, -50.0, AAP64ColorPalette.SkyBlue),
            new(-49.0, 0.0, AAP64ColorPalette.Cyan),
            new(1.0, 10.0, AAP64ColorPalette.Mint),
            new(11.0, 20.0, AAP64ColorPalette.White),
            new(21.0, 35.0, AAP64ColorPalette.PaleYellow),
            new(36.0, 50.0, AAP64ColorPalette.Gold),
            new(51.0, 100.0, AAP64ColorPalette.Orange),
            new(101.0, 200.0, AAP64ColorPalette.OrangeRed),
            new(201.0, 300.0, AAP64ColorPalette.Crimson),
            new(301.0, 500.0, AAP64ColorPalette.DarkRed),
            new(501.0, 1000.0, AAP64ColorPalette.Maroon),
            new(1001.0, 2000.0, AAP64ColorPalette.Brown),
            new(2001.0, 4000.0, AAP64ColorPalette.DarkBrown),
            new(4001.0, 9725.0, AAP64ColorPalette.DarkGray)
        ];

        internal static Color GetTemperatureColor(double temperature)
        {
            for (int i = 0; i < TEMPERATURE_COLOR_RANGES.Length; i++)
            {
                if (temperature >= TEMPERATURE_COLOR_RANGES[i].MinimumTemperature && temperature <= TEMPERATURE_COLOR_RANGES[i].MaximumTemperature)
                {
                    return TEMPERATURE_COLOR_RANGES[i].Color;
                }
            }

            return AAP64ColorPalette.DarkGray;
        }

        internal static Color ApplyHeatColor(Color baseColor, double temperature)
        {
            return baseColor.OverlayBlend(GetTemperatureColor(temperature), COLOR_HEAT_FACTOR);
        }
    }
}
