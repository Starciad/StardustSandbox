/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Extensions;

namespace StardustSandbox.Core.Constants
{
    internal static class TemperatureConstants
    {
        // Constants for simulation (can be tuned for realism)
        internal const float THERMAL_CONDUCTIVITY = 0.5f; // k, arbitrary unit
        internal const float WORLD_THERMAL_CONDUCTIVITY = 1.5f; // k, arbitrary unit
        internal const float AREA = 1.0f; // contact area, arbitrary unit
        internal const float DISTANCE = 1.0f; // distance between centers, arbitrary unit

        internal const float MIN_CELSIUS_VALUE = -275.0f;
        internal const float MAX_CELSIUS_VALUE = 9725.0f;
        internal const float EQUILIBRIUM_THRESHOLD = 1.0f;
        internal const float COLOR_HEAT_FACTOR = 0.45f;

        internal static readonly TemperatureColorRange[] TEMPERATURE_COLOR_RANGES =
        [
            new(-275.0f, -200.0f, AAP64ColorPalette.DarkTeal),
            new(-199.0f, -150.0f, AAP64ColorPalette.NavyBlue),
            new(-149.0f, -100.0f, AAP64ColorPalette.RoyalBlue),
            new(-99.0f, -50.0f, AAP64ColorPalette.SkyBlue),
            new(-49.0f, 0.0f, AAP64ColorPalette.Cyan),
            new(1.0f, 10.0f, AAP64ColorPalette.Mint),
            new(11.0f, 20.0f, AAP64ColorPalette.White),
            new(21.0f, 35.0f, AAP64ColorPalette.PaleYellow),
            new(36.0f, 50.0f, AAP64ColorPalette.Gold),
            new(51.0f, 100.0f, AAP64ColorPalette.Orange),
            new(101.0f, 200.0f, AAP64ColorPalette.OrangeRed),
            new(201.0f, 300.0f, AAP64ColorPalette.Crimson),
            new(301.0f, 500.0f, AAP64ColorPalette.DarkRed),
            new(501.0f, 1000.0f, AAP64ColorPalette.Maroon),
            new(1001.0f, 2000.0f, AAP64ColorPalette.Brown),
            new(2001.0f, 4000.0f, AAP64ColorPalette.DarkBrown),
            new(4001.0f, 9725.0f, AAP64ColorPalette.DarkGray)
        ];

        internal static Color GetTemperatureColor(float temperature)
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

        internal static Color ApplyHeatColor(Color baseColor, float temperature)
        {
            return baseColor.OverlayBlend(GetTemperatureColor(temperature), COLOR_HEAT_FACTOR);
        }
    }
}
