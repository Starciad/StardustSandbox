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

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Colors.Palettes;

using System;

namespace StardustSandbox.Core.Constants
{
    internal static class GradientConstants
    {
        private static readonly GradientColorMap[] backgroundGradientColorMap = [
            new()
            {
                StartTime = new(0, 0, 0), // Midnight
                EndTime = new(3, 0, 0),  // Late Night
                GradientStartColor = new(AAP64ColorPalette.DarkPurple, AAP64ColorPalette.NavyBlue),
                GradientEndColor = new(AAP64ColorPalette.NavyBlue, AAP64ColorPalette.DarkTeal),
            },

            new()
            {
                StartTime = new(3, 0, 0), // Late Night
                EndTime = new(6, 0, 0),  // Dawn
                GradientStartColor = new(AAP64ColorPalette.NavyBlue, AAP64ColorPalette.DarkTeal),
                GradientEndColor = new(AAP64ColorPalette.DarkTeal, AAP64ColorPalette.OrangeRed),
            },

            new()
            {
                StartTime = new(6, 0, 0), // Dawn
                EndTime = new(8, 0, 0),  // Early Morning
                GradientStartColor = new(AAP64ColorPalette.DarkTeal, AAP64ColorPalette.OrangeRed),
                GradientEndColor = new(AAP64ColorPalette.SkyBlue, AAP64ColorPalette.Orange),
            },

            new()
            {
                StartTime = new(8, 0, 0), // Early Morning
                EndTime = new(12, 0, 0), // Noon
                GradientStartColor = new(AAP64ColorPalette.SkyBlue, AAP64ColorPalette.Orange),
                GradientEndColor = new(AAP64ColorPalette.SkyBlue, AAP64ColorPalette.LemonYellow),
            },

            new()
            {
                StartTime = new(12, 0, 0), // Noon
                EndTime = new(15, 0, 0),  // Early Afternoon
                GradientStartColor = new(AAP64ColorPalette.SkyBlue, AAP64ColorPalette.LemonYellow),
                GradientEndColor = new(AAP64ColorPalette.SkyBlue, AAP64ColorPalette.Gold),
            },

            new()
            {
                StartTime = new(15, 0, 0), // Early Afternoon
                EndTime = new(18, 0, 0),  // Dusk
                GradientStartColor = new(AAP64ColorPalette.SkyBlue, AAP64ColorPalette.Gold),
                GradientEndColor = new(AAP64ColorPalette.OrangeRed, AAP64ColorPalette.DarkTeal),
            },

            new()
            {
                StartTime = new(18, 0, 0), // Dusk
                EndTime = new(20, 0, 0), // Evening
                GradientStartColor = new(AAP64ColorPalette.OrangeRed, AAP64ColorPalette.DarkTeal),
                GradientEndColor = new(AAP64ColorPalette.DarkTeal, AAP64ColorPalette.NavyBlue),
            },

            new()
            {
                StartTime = new(20, 0, 0), // Evening
                EndTime = new(24, 0, 0), // End of day (24:00:00)
                GradientStartColor = new(AAP64ColorPalette.DarkTeal, AAP64ColorPalette.NavyBlue),
                GradientEndColor = new(AAP64ColorPalette.DarkPurple, AAP64ColorPalette.NavyBlue),
            },
        ];

        internal static GradientColorMap GetBackgroundGradientByTime(TimeSpan currentTime)
        {
            return Array.Find(backgroundGradientColorMap, x =>
            {
                return currentTime >= x.StartTime && currentTime < x.EndTime;
            }) ?? backgroundGradientColorMap[^1];
        }
    }
}

