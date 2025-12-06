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

                Point displacement = direction switch
                {
                    CardinalDirection.North => new(0, -1),
                    CardinalDirection.South => new(0, 1),
                    CardinalDirection.East => new(1, 0),
                    CardinalDirection.West => new(-1, 0),
                    _ => Point.Zero,
                };

                context.UpdateElementPosition(neighbors.GetSlot(i).Position, neighbors.GetSlot(i).Position + displacement);
            }

        }
    }
}
