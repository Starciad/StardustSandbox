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

using StardustSandbox.Core;
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.Elements;

using System.Collections.Generic;

namespace StardustSandbox.Core.Elements.Utilities
{
    internal static class ElementUtility
    {
        internal static IEnumerable<Point> GetRandomSidePositions(Point targetPosition, Direction direction)
        {
            int rDirection = Random.GetBool() ? 1 : -1;

            switch (direction)
            {
                case Direction.Up:
                    yield return new(targetPosition.X, targetPosition.Y - 1);
                    yield return new(targetPosition.X + rDirection, targetPosition.Y - 1);
                    yield return new(targetPosition.X + (rDirection * -1), targetPosition.Y - 1);
                    break;

                case Direction.Left:
                    yield return new(targetPosition.X + 1, targetPosition.Y);
                    yield return new(targetPosition.X + 1, targetPosition.Y + rDirection);
                    yield return new(targetPosition.X + 1, targetPosition.Y + (rDirection * -1));
                    break;

                case Direction.Down:
                    yield return new(targetPosition.X, targetPosition.Y + 1);
                    yield return new(targetPosition.X + rDirection, targetPosition.Y + 1);
                    yield return new(targetPosition.X + (rDirection * -1), targetPosition.Y + 1);
                    break;

                case Direction.Right:
                    yield return new(targetPosition.X - 1, targetPosition.Y);
                    yield return new(targetPosition.X - 1, targetPosition.Y + rDirection);
                    yield return new(targetPosition.X - 1, targetPosition.Y + (rDirection * -1));
                    break;

                default:
                    yield return new(targetPosition.X, targetPosition.Y + 1);
                    yield return new(targetPosition.X + rDirection, targetPosition.Y + 1);
                    yield return new(targetPosition.X + (rDirection * -1), targetPosition.Y + 1);
                    break;
            }
        }

        internal static void NotifyFreeFallingFromAdjacentNeighbors(ElementContext context, Point position)
        {
            context.SetElementState(new(position.X - 1, position.Y), context.Layer, ElementStates.IsFalling);
            context.SetElementState(new(position.X + 1, position.Y), context.Layer, ElementStates.IsFalling);
        }
    }
}

