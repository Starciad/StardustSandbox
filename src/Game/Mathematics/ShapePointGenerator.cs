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
using System.Collections.Generic;

namespace StardustSandbox.Mathematics
{
    internal static class ShapePointGenerator
    {
        internal static IEnumerable<Point> EnumerateCirclePoints(Point center, int radius)
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

        internal static IEnumerable<Point> EnumerateSquarePoints(Point center, int radius)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    yield return new(center.X + x, center.Y + y);
                }
            }
        }

        internal static IEnumerable<Point> EnumerateTrianglePoints(Point center, int height)
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

        /// <summary>
        /// Enumerates all integer grid points that form a straight line between two points.
        /// </summary>
        /// <remarks>
        /// This method is based on the classic Bresenham's Line Algorithm,
        /// originally developed by Jack E. Bresenham in 1962 for efficient
        /// rasterization of lines on discrete grids using only integer arithmetic.
        /// </remarks>
        internal static IEnumerable<Point> EnumerateLinePoints(Point startPoint, Point endPoint)
        {
            int currentX = startPoint.X;
            int currentY = startPoint.Y;

            int targetX = endPoint.X;
            int targetY = endPoint.Y;

            int deltaX = Math.Abs(targetX - currentX);
            int deltaY = Math.Abs(targetY - currentY);

            int stepX = currentX < targetX ? 1 : -1;
            int stepY = currentY < targetY ? 1 : -1;

            int error = deltaX - deltaY;

            while (true)
            {
                yield return new(currentX, currentY);

                if (currentX == targetX && currentY == targetY)
                {
                    break;
                }

                int doubledError = error * 2;

                if (doubledError > -deltaY)
                {
                    error -= deltaY;
                    currentX += stepX;
                }

                if (doubledError < deltaX)
                {
                    error += deltaX;
                    currentY += stepY;
                }
            }
        }
    }
}

