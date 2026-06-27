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
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Enums.Directions;

namespace StardustSandbox.Core.Mathematics
{
    internal static class OriginMath
    {
        internal static Vector2 GetSpriteFontOriginPoint(this SpriteFont spriteFont, string text, UIAlignment direction)
        {
            Vector2 measuredString = spriteFont.MeasureString(text);
            return GetOriginPoint(new(measuredString.X, measuredString.Y), direction);
        }

        internal static Vector2 GetOriginPoint(Vector2 size, UIAlignment direction)
        {
            return direction switch
            {
                // (.)
                UIAlignment.Center => size / 2f,

                // (↑)
                UIAlignment.North => new(size.X / 2f, size.Y),

                // (↗)
                UIAlignment.Northeast => new(0f, size.Y),

                // (→)
                UIAlignment.East => new(0f, size.Y / 2f),

                // (↘)
                UIAlignment.Southeast => new(0f, 0f),

                // (↓)
                UIAlignment.South => new(size.X / 2f, 0f),

                // (↙)
                UIAlignment.Southwest => new(size.X, 0f),

                // (←)
                UIAlignment.West => new(size.X, size.Y / 2f),

                // (↖)
                UIAlignment.Northwest => new(size.X, size.Y),

                // (.)
                _ => new(size.X, size.Y),
            };
        }
    }
}
