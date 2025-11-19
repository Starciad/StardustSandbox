using Microsoft.Xna.Framework;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Extensions;

namespace StardustSandbox.Constants
{
    internal static class TemperatureConstants
    {
        internal const short MIN_CELSIUS_VALUE = -275;
        internal const short MAX_CELSIUS_VALUE = 9725;
        internal const short EQUILIBRIUM_THRESHOLD = 1;
        internal const float COLOR_HEAT_FACTOR = 0.45f;

        internal static readonly (short min, short max, Color color)[] TEMPERATURE_COLOR_RANGES =
        [
            (-275, -200, AAP64ColorPalette.DarkTeal),
            (-199, -150, AAP64ColorPalette.NavyBlue),
            (-149, -100, AAP64ColorPalette.RoyalBlue),
            (-99, -50, AAP64ColorPalette.SkyBlue),
            (-49, 0, AAP64ColorPalette.Cyan),
            (1, 10, AAP64ColorPalette.Mint),
            (11, 20, AAP64ColorPalette.White),
            (21, 35, AAP64ColorPalette.PaleYellow),
            (36, 50, AAP64ColorPalette.Gold),
            (51, 100, AAP64ColorPalette.Orange),
            (101, 200, AAP64ColorPalette.OrangeRed),
            (201, 300, AAP64ColorPalette.Crimson),
            (301, 500, AAP64ColorPalette.DarkRed),
            (501, 1000, AAP64ColorPalette.Maroon),
            (1001, 2000, AAP64ColorPalette.Brown),
            (2001, 4000, AAP64ColorPalette.DarkBrown),
            (4001, 9725, AAP64ColorPalette.DarkGray)
        ];

        internal static Color GetTemperatureColor(float temperature)
        {
            foreach ((short min, short max, Color color) in TEMPERATURE_COLOR_RANGES)
            {
                if (temperature >= min && temperature <= max)
                {
                    return color;
                }
            }

            return AAP64ColorPalette.DarkGray;
        }

        internal static Color ApplyHeatColor(Color baseColor, float temperature)
        {
            return baseColor.OverlayBlend(GetTemperatureColor(temperature), COLOR_HEAT_FACTOR);
        }
    }
}
