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

using System;

namespace StardustSandbox.Core.Constants
{
    internal static class BackgroundConstants
    {
        internal static Vector2 CELESTIAL_BODY_CENTER_PIVOT => new(ScreenConstants.SCREEN_WIDTH / 2.0f, ScreenConstants.SCREEN_HEIGHT);

        internal const byte MAX_SIMULTANEOUS_CLOUDS = 32;

        internal const float CELESTIAL_BODY_MAX_ARC_ANGLE = MathF.PI;
        internal const float CELESTIAL_BODY_ARC_OFFSET = MathF.PI * 2.0f;
        internal const float CELESTIAL_BODY_ARC_RADIUS = 500.0f;
    }
}

