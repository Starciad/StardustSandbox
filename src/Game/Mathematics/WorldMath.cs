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

using Microsoft.Xna.Framework;

using StardustSandbox.Constants;

namespace StardustSandbox.Mathematics
{
    internal static class WorldMath
    {
        internal static Vector2 ToWorldPosition(Vector2 globalPosition)
        {
            return new(
                (int)(globalPosition.X / WorldConstants.GRID_SIZE),
                (int)(globalPosition.Y / WorldConstants.GRID_SIZE)
            );
        }

        internal static Vector2 ToGlobalPosition(Vector2 worldPosition)
        {
            return new(
                (int)(worldPosition.X * WorldConstants.GRID_SIZE),
                (int)(worldPosition.Y * WorldConstants.GRID_SIZE)
            );
        }
    }
}

