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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.Elements;

using System;

namespace StardustSandbox.Core.Elements.Utilities
{
    internal static class PusherUtility
    {
        private delegate Point DirectionSelector(Point neighborPosition);

        internal static void PushingNeighborsUp(ElementContext context, ElementNeighbors neighbors)
        {
            PushNeighbors(context, neighbors,
                frontDirection: p => new(p.X, p.Y - 1),
                leftDirection: p => new(p.X - 1, p.Y),
                rightDirection: p => new(p.X + 1, p.Y),
                isBehind: (neighbor, pusher) => neighbor.X == pusher.X && (neighbor.Y - 1) == pusher.Y
            );
        }

        internal static void PushingNeighborsRight(ElementContext context, ElementNeighbors neighbors)
        {
            PushNeighbors(context, neighbors,
                frontDirection: p => new(p.X + 1, p.Y),
                leftDirection: p => new(p.X, p.Y - 1),
                rightDirection: p => new(p.X, p.Y + 1),
                isBehind: (neighbor, pusher) => neighbor.Y == pusher.Y && (neighbor.X + 1) == pusher.X
            );
        }

        internal static void PushingNeighborsDown(ElementContext context, ElementNeighbors neighbors)
        {
            PushNeighbors(context, neighbors,
                frontDirection: p => new(p.X, p.Y + 1),
                leftDirection: p => new(p.X - 1, p.Y),
                rightDirection: p => new(p.X + 1, p.Y),
                isBehind: (neighbor, pusher) => neighbor.X == pusher.X && (neighbor.Y + 1) == pusher.Y
            );
        }

        internal static void PushingNeighborsLeft(ElementContext context, ElementNeighbors neighbors)
        {
            PushNeighbors(context, neighbors,
                frontDirection: p => new(p.X - 1, p.Y),
                leftDirection: p => new(p.X, p.Y - 1),
                rightDirection: p => new(p.X, p.Y + 1),
                isBehind: (neighbor, pusher) => neighbor.Y == pusher.Y && (neighbor.X - 1) == pusher.X
            );
        }

        private static void PushNeighbors(
            ElementContext context,
            ElementNeighbors neighbors,
            DirectionSelector frontDirection,
            DirectionSelector leftDirection,
            DirectionSelector rightDirection,
            Func<Point, Point, bool> isBehind
        )
        {
            for (int i = 0; i < ElementConstants.NEIGHBORS_ARRAY_LENGTH; i++)
            {
                ElementNeighborDirection direction = (ElementNeighborDirection)i;

                if (direction is not (ElementNeighborDirection.North or ElementNeighborDirection.West or ElementNeighborDirection.East or ElementNeighborDirection.South) ||
                    !neighbors.IsNeighborLayerOccupied(i, context.Layer) ||
                    !neighbors.GetSlotLayer(i, context.Layer).Element.HasCharacteristic(ElementCharacteristics.IsPushable) ||
                     neighbors.GetSlotLayer(i, context.Layer).HasState(ElementStates.WasPushed))
                {
                    continue;
                }

                Point pusherPosition = context.Position;
                Point currentNeighborPosition = neighbors.GetSlot(i).Position;
                Point targetNeighborPosition = currentNeighborPosition;

                Point frontPosition = frontDirection(currentNeighborPosition);
                Point leftPosition = leftDirection(currentNeighborPosition);
                Point rightPosition = rightDirection(currentNeighborPosition);

                bool frontEmpty = context.IsEmptySlotLayer(frontPosition);
                bool leftEmpty = context.IsEmptySlotLayer(leftPosition);
                bool rightEmpty = context.IsEmptySlotLayer(rightPosition);
                bool wasPushed = false;

                if (isBehind(currentNeighborPosition, pusherPosition))
                {
                    if (leftEmpty && rightEmpty)
                    {
                        targetNeighborPosition = Randomness.Random.GetBool() ? leftPosition : rightPosition;
                        wasPushed = true;
                    }
                    else if (leftEmpty)
                    {
                        targetNeighborPosition = leftPosition;
                        wasPushed = true;
                    }
                    else if (rightEmpty)
                    {
                        targetNeighborPosition = rightPosition;
                        wasPushed = true;
                    }
                }
                else if (frontEmpty)
                {
                    targetNeighborPosition = frontPosition;
                    wasPushed = true;
                }

                if (wasPushed)
                {
                    GameStatistics.IncrementWorldPushedElements();
                }

                if (context.TryUpdateElementPosition(currentNeighborPosition, targetNeighborPosition))
                {
                    context.RemoveElementState(currentNeighborPosition, ElementStates.IsFalling);
                    context.SetElementState(targetNeighborPosition, ElementStates.WasPushed);
                }
            }

            if (neighbors.CountOccupied > 0)
            {
                context.NotifyChunk(context.Position);
            }
        }
    }
}

