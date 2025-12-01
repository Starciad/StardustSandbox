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

        protected override void OnBeforeStep(ElementContext context)
        {
            positionBuffer.Clear();
            layerBuffer.Clear();
        }

        protected override void OnAfterStep(ElementContext context)
        {
            TryInstantiateStoredElement(context);
        }

        protected override void OnNeighbors(ElementContext context, IEnumerable<Slot> neighbors)
        {
            TryDefineStoredElement(context, neighbors);
        }

        private static void TryInstantiateStoredElement(ElementContext context)
        {
            if (context.SlotLayer.StoredElement == null || !TryGetValidPosition(context, out Point validPositon))
            {
                return;
            }

            context.InstantiateElement(validPositon, context.Layer, context.SlotLayer.StoredElement);
        }

        private static bool TryGetValidPosition(ElementContext context, out Point validPosition)
        {
            foreach (Point position in context.Slot.Position.GetNeighboringCardinalPoints())
            {
                if (context.IsEmptySlotLayer(position, context.Layer))
                {
                    positionBuffer.Add(position);
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

        private static void TryDefineStoredElement(ElementContext context, IEnumerable<Slot> neighbors)
        {
            if (context.SlotLayer.StoredElement != null)
            {
                return;
            }

            foreach (Slot neighbor in neighbors)
            {
                SlotLayer layer = neighbor.GetLayer(context.Layer);

                if (layer.HasState(ElementStates.IsEmpty) ||
                    layer.Element.Index == ElementIndex.Clone ||
                    layer.Element.Index == ElementIndex.Wall ||
                    layer.Element.Index == ElementIndex.Void)
                {
                    continue;
                }

                layerBuffer.Add(layer);
            }

            if (layerBuffer.Count == 0)
            {
                return;
            }

            context.SetStoredElement(layerBuffer.GetRandomItem().Element);
        }
    }
}
