/*
 * Copyright (C) 2026  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
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

namespace StardustSandbox.Enums.Elements
{
    internal enum SpriteKeyPoints : byte
    {
        // Full
        Full_Northwest = 0,
        Full_Northeast = 1,
        Full_Southwest = 2,
        Full_Southeast = 3,

        // Corner
        Corner_Northwest = 4,
        Corner_Northeast = 5,
        Corner_Southwest = 6,
        Corner_Southeast = 7,

        // Vertical Edge
        Vertical_Edge_Northwest = 8,
        Vertical_Edge_Northeast = 9,
        Vertical_Edge_Southwest = 10,
        Vertical_Edge_Southeast = 11,

        // Horizontal Border
        Horizontal_Edge_Northwest = 12,
        Horizontal_Edge_Northeast = 13,
        Horizontal_Edge_Southwest = 14,
        Horizontal_Edge_Southeast = 15,

        // Gaps
        Gap_Northwest = 16,
        Gap_Northeast = 17,
        Gap_Southwest = 18,
        Gap_Southeast = 19,
    }
}

