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

namespace StardustSandbox.Core.Constants
{
    internal static class WorldConstants
    {
        internal static Point WORLD_THUMBNAIL_SIZE => new(23);

        internal const byte GRID_SIZE = 32;
        internal const byte CHUNK_SCALE = 6;
        internal const byte CHUNK_DEFAULT_COOLDOWN = 3;
        internal const float BACKGROUND_COLOR_DARKENING_FACTOR = 0.5f;

        internal static readonly Point[] WORLD_SIZES_TEMPLATE =
        [
            // (0) 40x23 (920 elements) - Fits entirely within the player's camera
            new(40, 23), // Small

            // (1) 80x46 (3,680 elements)
            new(80, 46), // Medium-Small

            // (2) 120x69 (8,280 elements)
            new(120, 69), // Medium

            // (3) 160x92 (14,720 elements)
            new(160, 92), // Medium-Large

            // (4) 240x138 (33,120 elements)
            new(240, 138), // Large

            // (5) 320x184 (58,880 elements)
            new(320, 184), // Very Large
        ];
    }
}

