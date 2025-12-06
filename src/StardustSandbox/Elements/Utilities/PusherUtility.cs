using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Utilities
{
    internal static class PusherUtility
    {
        internal static void Push(in ElementContext context, in ElementNeighbors neighbors, CardinalDirection direction)
        {
            // Find pushable neighbors
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.HasNeighbor(i) ||
                    neighbors.GetSlotLayer(i, context.Layer).HasState(ElementStates.IsEmpty) ||
                    !neighbors.GetSlotLayer(i, context.Layer).Element.Characteristics.HasFlag(ElementCharacteristics.IsPushable))
                {
                    continue;
                }

                Point targetPosition = neighbors.GetSlot(i).Position;

                switch (direction)
                {
                    case CardinalDirection.North:
                        targetPosition += new Point(0, -1);
                        break;

                    case CardinalDirection.South:
                        targetPosition += new Point(0, 1);
                        break;

                    case CardinalDirection.East:
                        targetPosition += new Point(1, 0);
                        break;

                    case CardinalDirection.West:
                        targetPosition += new Point(-1, 0);
                        break;

                    default:
                        break;
                }

                context.UpdateElementPosition(neighbors.GetSlot(i).Position, targetPosition);
            }

        }
    }
}
