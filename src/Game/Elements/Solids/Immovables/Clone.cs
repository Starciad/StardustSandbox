using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;
using StardustSandbox.Extensions;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Clone : ImmovableSolid
    {
        private static readonly List<Point> positionScratch = [];
        private static readonly List<SlotLayer> layerScratch = [];

        protected override void OnBeforeStep(ElementContext context)
        {
            positionScratch.Clear();
            layerScratch.Clear();
        }

        // Instantiate the stored element into a random valid adjacent empty slot
        private static void TryInstantiateStoredElement(ElementContext context)
        {
            ElementIndex stored = context.GetStoredElement();

            if (stored == ElementIndex.None)
            {
                return;
            }

            if (!TryGetValidPosition(context, out Point validPosition))
            {
                return;
            }

            context.InstantiateElement(validPosition, context.Layer, stored);
        }

        private static void TryAddEmptyPosition(ElementContext context, Point position)
        {
            if (context.IsEmptySlotLayer(position, context.Layer))
            {
                positionScratch.Add(position);
            }
        }

        // Collect neighboring empty positions and pick one at random
        private static bool TryGetValidPosition(ElementContext context, out Point validPosition)
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

            if (positionScratch.Count == 0)
            {
                validPosition = Point.Zero;
                return false;
            }

            validPosition = positionScratch.GetRandomItem();
            return true;
        }

        // Define the stored element based on the first valid neighboring element found
        private static void TryDefineStoredElement(ElementContext context, ElementNeighbors neighbors)
        {
            if (context.GetStoredElement() != ElementIndex.None)
            {
                return;
            }

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                SlotLayer neighborLayer = neighbors.GetSlotLayer(i, context.Layer);
                ElementIndex index = neighborLayer.ElementIndex;

                // Skip cloning from these element types
                switch (index)
                {
                    case ElementIndex.Clone:
                    case ElementIndex.Wall:
                    case ElementIndex.Void:
                    case ElementIndex.LightningBody:
                    case ElementIndex.LightningHead:
                        continue;

                    default:
                        layerScratch.Add(neighborLayer);
                        break;
                }

                layerScratch.Add(neighborLayer);
            }

            if (layerScratch.Count == 0)
            {
                return;
            }

            context.SetStoredElement(layerScratch.GetRandomItem().ElementIndex);
        }

        protected override void OnAfterStep(ElementContext context)
        {
            TryInstantiateStoredElement(context);
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            TryDefineStoredElement(context, neighbors);
        }
    }
}
