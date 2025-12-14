using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;
using StardustSandbox.Extensions;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Clone : ImmovableSolid
    {
        private static readonly List<Point> positionBuffer = [];
        private static readonly List<SlotLayer> layerBuffer = [];

        protected override void OnBeforeStep(in ElementContext context)
        {
            positionBuffer.Clear();
            layerBuffer.Clear();
        }

        protected override void OnAfterStep(in ElementContext context)
        {
            TryInstantiateStoredElement(context);
        }

        protected override void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors)
        {
            TryDefineStoredElement(context, neighbors);
        }

        private static void TryInstantiateStoredElement(in ElementContext context)
        {
            if (context.SlotLayer.StoredElement == null || !TryGetValidPosition(context, out Point validPositon))
            {
                return;
            }

            context.InstantiateElement(validPositon, context.Layer, context.SlotLayer.StoredElement);
        }

        private static void TryAddEmptyPosition(in ElementContext context, Point position)
        {
            if (context.IsEmptySlotLayer(position, context.Layer))
            {
                positionBuffer.Add(position);
            }
        }

        private static bool TryGetValidPosition(in ElementContext context, out Point validPosition)
        {
            int centerX = context.Slot.Position.X;
            int centerY = context.Slot.Position.Y;

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                    {
                        continue;
                    }

                    TryAddEmptyPosition(context, new(centerX + dx, centerY + dy));
                }
            }

            if (positionBuffer.Count == 0)
            {
                validPosition = Point.Zero;
                return false;
            }

            validPosition = positionBuffer.GetRandomItem();
            return true;
        }

        private static void TryDefineStoredElement(in ElementContext context, in ElementNeighbors neighbors)
        {
            if (context.SlotLayer.StoredElement != null)
            {
                return;
            }

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                switch (neighbors.GetSlotLayer(i, context.Layer).Element.Index)
                {
                    case ElementIndex.Clone:
                    case ElementIndex.Wall:
                    case ElementIndex.Void:
                    case ElementIndex.LightningBody:
                    case ElementIndex.LightningHead:
                        continue;

                    default:
                        break;
                }

                layerBuffer.Add(neighbors.GetSlotLayer(i, context.Layer));
            }

            if (layerBuffer.Count == 0)
            {
                return;
            }

            context.SetStoredElement(layerBuffer.GetRandomItem().Element);
        }
    }
}
