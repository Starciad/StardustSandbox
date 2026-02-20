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

using StardustSandbox.Core.Colors.Palettes;

namespace StardustSandbox.Core.Constants
{
    internal static class ElementConstants
    {
        #region GENERAL

        internal const byte NEIGHBORS_ARRAY_LENGTH = 8;

        #endregion

        #region SETTINGS

        // Corruption
        internal const byte CHANCE_OF_CORRUPTION_TO_SPREAD = 4;

        // Fire
        internal const byte CHANCE_OF_FIRE_TO_DISAPPEAR = 20;
        internal const byte CHANCE_FOR_FIRE_TO_LEAVE_SMOKE = 25;

        internal const byte FIRE_HEAT_VALUE = 5;
        internal const byte CHANCE_OF_COMBUSTION = 25;

        // Mounting Block
        internal static readonly Color[] COLORS_OF_MOUNTING_BLOCKS = [
            AAP64ColorPalette.NavyBlue, // Blue
            AAP64ColorPalette.DarkGreen, // Green
            AAP64ColorPalette.DarkRed, // Red
            AAP64ColorPalette.LemonYellow, // Yellow
            AAP64ColorPalette.Violet, // Pink
        ];

        #endregion

        #region RENDERING

        #region SPRITES

        internal const byte SPRITE_DIVISIONS_LENGTH = 4;
        internal const byte SPRITE_SLICE_SIZE = 16;

        internal const float SPRITE_X_OFFSET = 0.5f;
        internal const float SPRITE_Y_OFFSET = 0.5f;

        #endregion

        #region BLOB SYSTEM

        /*
         * WARNING!
         * 
         * Be very careful with the constants below. The constant values below
         * were calculated precisely so the system could slice the sprites
         * as planned. Any changes may result in system failure or malfunction.
         * 
         * The sprites that use the blob renderer must comply with the game's 
         * size and texturing standards, otherwise, they will not be compatible 
         * with the project and might be misinterpreted by the system.
         * 
         * Thank you in advance!
         * 
         */

        internal const int BLOB_ROTATION_VALUE = 4;

        // (Sprite 1 - Northwest Pivot)
        internal const byte BLOB_NORTHWEST_PIVOT_EMPTY = 0;
        internal const byte BLOB_NORTHWEST_PIVOT_CASE_1 = 64;
        internal const byte BLOB_NORTHWEST_PIVOT_CASE_2 = 128;
        internal const byte BLOB_NORTHWEST_PIVOT_CASE_3 = 1;
        internal const byte BLOB_NORTHWEST_PIVOT_CASE_4 = 192;
        internal const byte BLOB_NORTHWEST_PIVOT_CASE_5 = 129;
        internal const byte BLOB_NORTHWEST_PIVOT_CASE_6 = 65;
        internal const byte BLOB_NORTHWEST_PIVOT_SURROUNDED = 193;

        // (Sprite 2 - Northeast Pivot)
        internal const byte BLOB_NORTHEAST_PIVOT_EMPTY = 0;
        internal const byte BLOB_NORTHEAST_PIVOT_CASE_1 = 4;
        internal const byte BLOB_NORTHEAST_PIVOT_CASE_2 = 2;
        internal const byte BLOB_NORTHEAST_PIVOT_CASE_3 = 1;
        internal const byte BLOB_NORTHEAST_PIVOT_CASE_4 = 6;
        internal const byte BLOB_NORTHEAST_PIVOT_CASE_5 = 3;
        internal const byte BLOB_NORTHEAST_PIVOT_CASE_6 = 5;
        internal const byte BLOB_NORTHEAST_PIVOT_SURROUNDED = 7;

        // (Sprite 3 - Southwest Pivot)
        internal const byte BLOB_SOUTHWEST_PIVOT_EMPTY = 0;
        internal const byte BLOB_SOUTHWEST_PIVOT_CASE_1 = 64;
        internal const byte BLOB_SOUTHWEST_PIVOT_CASE_2 = 32;
        internal const byte BLOB_SOUTHWEST_PIVOT_CASE_3 = 16;
        internal const byte BLOB_SOUTHWEST_PIVOT_CASE_4 = 96;
        internal const byte BLOB_SOUTHWEST_PIVOT_CASE_5 = 48;
        internal const byte BLOB_SOUTHWEST_PIVOT_CASE_6 = 80;
        internal const byte BLOB_SOUTHWEST_PIVOT_SURROUNDED = 112;

        // (Sprite 4 - Southeast Pivot)
        internal const byte BLOB_SOUTHEAST_PIVOT_EMPTY = 0;
        internal const byte BLOB_SOUTHEAST_PIVOT_CASE_1 = 4;
        internal const byte BLOB_SOUTHEAST_PIVOT_CASE_2 = 8;
        internal const byte BLOB_SOUTHEAST_PIVOT_CASE_3 = 16;
        internal const byte BLOB_SOUTHEAST_PIVOT_CASE_4 = 12;
        internal const byte BLOB_SOUTHEAST_PIVOT_CASE_5 = 24;
        internal const byte BLOB_SOUTHEAST_PIVOT_CASE_6 = 20;
        internal const byte BLOB_SOUTHEAST_PIVOT_SURROUNDED = 28;

        #endregion

        #endregion
    }
}
