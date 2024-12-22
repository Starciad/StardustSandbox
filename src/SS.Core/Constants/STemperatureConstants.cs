using Microsoft.Xna.Framework;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Extensions;

using System;

namespace StardustSandbox.Core.Constants
{
    public static class STemperatureConstants
    {
        public const short MIN_CELSIUS_VALUE = -275;
        public const short MAX_CELSIUS_VALUE = 9725;
        public const short EQUILIBRIUM_THRESHOLD = 1;
        public const float COLOR_HEAT_FACTOR = 0.45f;

        public static readonly (short min, short max, Color color)[] TEMPERATURE_COLOR_RANGES =
        [
            (-275, -200, SColorPalette.DarkTeal),
            (-199, -150, SColorPalette.NavyBlue),
            (-149, -100, SColorPalette.RoyalBlue),
            (-99, -50, SColorPalette.SkyBlue),
            (-49, 0, SColorPalette.Cyan),
            (1, 10, SColorPalette.Mint),
            (11, 20, SColorPalette.White),
            (21, 35, SColorPalette.PaleYellow),
            (36, 50, SColorPalette.Gold),
            (51, 100, SColorPalette.Orange),
            (101, 200, SColorPalette.OrangeRed),
            (201, 300, SColorPalette.Crimson),
            (301, 500, SColorPalette.DarkRed),
            (501, 1000, SColorPalette.Maroon),
            (1001, 2000, SColorPalette.Brown),
            (2001, 4000, SColorPalette.DarkBrown),
            (4001, 9725, SColorPalette.DarkGray)
        ];

        public static Color GetTemperatureColor(float temperature)
        {
            foreach ((short min, short max, Color color) in TEMPERATURE_COLOR_RANGES)
            {
                if (temperature >= min && temperature <= max)
                {
                    return color;
                }
            }

            return SColorPalette.DarkGray;
        }

        public static Color ApplyHeatColor(Color baseColor, float temperature)
        {
            return baseColor.OverlayBlend(GetTemperatureColor(temperature), COLOR_HEAT_FACTOR);
        }
    }
}
