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

using System.Collections.Generic;

namespace StardustSandbox.Mathematics
{
    internal static class ShapePointGenerator
    {
        internal static IEnumerable<Point> GenerateCirclePoints(Point center, int radius)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    if ((x * x) + (y * y) <= radius * radius)
                    {
                        yield return new(center.X + x, center.Y + y);
                    }
                }
            }
        }

        internal static IEnumerable<Point> GenerateSquarePoints(Point center, int radius)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    yield return new(center.X + x, center.Y + y);
                }
            }
        }

        internal static IEnumerable<Point> GenerateTrianglePoints(Point center, int height)
        {
            for (int y = 0; y <= height; y++)
            {
                int rowWidth = height - y;
                for (int x = -rowWidth; x <= rowWidth; x++)
                {
                    yield return new(center.X + x, center.Y - y + (height / 2));
                }
            }
        }
    }
}

