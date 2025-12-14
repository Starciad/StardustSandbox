using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;
using StardustSandbox.Randomness;

using System;

namespace StardustSandbox.Elements.Utilities
{
    internal static class PusherUtility
    {
        private delegate Point DirectionSelector(Point neighborPosition);

        internal static void PushingNeighborsUp(in ElementContext context, in ElementNeighbors neighbors)
        {
            PushNeighbors(context, neighbors,
                frontDirection: p => new Point(p.X, p.Y - 1),
                leftDirection: p => new Point(p.X - 1, p.Y),
                rightDirection: p => new Point(p.X + 1, p.Y),
                isBehind: (neighbor, pusher) => neighbor.X == pusher.X && (neighbor.Y - 1) == pusher.Y
            );
        }

        internal static void PushingNeighborsRight(in ElementContext context, in ElementNeighbors neighbors)
        {
            PushNeighbors(context, neighbors,
                frontDirection: p => new Point(p.X + 1, p.Y),
                leftDirection: p => new Point(p.X, p.Y - 1),
                rightDirection: p => new Point(p.X, p.Y + 1),
                isBehind: (neighbor, pusher) => neighbor.Y == pusher.Y && (neighbor.X + 1) == pusher.X
            );
        }

        internal static void PushingNeighborsDown(in ElementContext context, in ElementNeighbors neighbors)
        {
            PushNeighbors(context, neighbors,
                frontDirection: p => new Point(p.X, p.Y + 1),
                leftDirection: p => new Point(p.X - 1, p.Y),
                rightDirection: p => new Point(p.X + 1, p.Y),
                isBehind: (neighbor, pusher) => neighbor.X == pusher.X && (neighbor.Y + 1) == pusher.Y
            );
        }

        internal static void PushingNeighborsLeft(in ElementContext context, in ElementNeighbors neighbors)
        {
            PushNeighbors(context, neighbors,
                frontDirection: p => new Point(p.X - 1, p.Y),
                leftDirection: p => new Point(p.X, p.Y - 1),
                rightDirection: p => new Point(p.X, p.Y + 1),
                isBehind: (neighbor, pusher) => neighbor.Y == pusher.Y && (neighbor.X - 1) == pusher.X
            );
        }

        private static void PushNeighbors(
            in ElementContext context,
            in ElementNeighbors neighbors,
            DirectionSelector frontDirection,
            DirectionSelector leftDirection,
            DirectionSelector rightDirection,
            Func<Point, Point, bool> isBehind
        )
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer) ||
                    !neighbors.GetSlotLayer(i, context.Layer).Element.Characteristics.HasFlag(ElementCharacteristics.IsPushable))
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

                if (isBehind(currentNeighborPosition, pusherPosition))
                {
                    if (leftEmpty && rightEmpty)
                    {
                        targetNeighborPosition = SSRandom.GetBool() ? leftPosition : rightPosition;
                    }
                    else if (leftEmpty)
                    {
                        targetNeighborPosition = leftPosition;
                    }
                    else if (rightEmpty)
                    {
                        targetNeighborPosition = rightPosition;
                    }
                }
                else if (frontEmpty)
                {
                    targetNeighborPosition = frontPosition;
                }

                context.UpdateElementPosition(currentNeighborPosition, targetNeighborPosition);
            }
        }
    }
}
