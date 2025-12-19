using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;
using StardustSandbox.Extensions;
using StardustSandbox.Randomness;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Gases
{
    internal sealed class AntiCorruption : Gas
    {
        private static readonly List<Slot> cachedCorruptionNeighborSlots = [];

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            cachedCorruptionNeighborSlots.Clear();

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                Slot slot = neighbors.GetSlot(i);

                if (slot.GetLayer(context.Layer).Element.Characteristics.HasFlag(ElementCharacteristics.IsCorruption))
                {
                    cachedCorruptionNeighborSlots.Add(slot);
                }
            }

            if (cachedCorruptionNeighborSlots.Count > 0)
            {
                Slot corruptionNeighborSlot = cachedCorruptionNeighborSlots.GetRandomItem();
                SlotLayer neighborCorruptionSlotLayer = corruptionNeighborSlot.GetLayer(context.Layer);

                Element currentStoredElement = context.GetStoredElement();
                Element neighborStoredElement = neighborCorruptionSlotLayer.StoredElement;

                Point oldPosition = context.Slot.Position;
                Point newPosition = corruptionNeighborSlot.Position;

                context.SwappingElements(oldPosition, newPosition, context.Layer);

                if (currentStoredElement == null)
                {
                    context.ReplaceElement(oldPosition, ElementIndex.AntiCorruption);
                }
                else
                {
                    context.ReplaceElement(oldPosition, currentStoredElement);
                }

                context.SetStoredElement(newPosition, neighborStoredElement);
            }
            else if (context.GetStoredElement() != null)
            {
                context.ReplaceElement(context.GetStoredElement());
            }
            else if (SSRandom.Chance(15))
            {
                context.DestroyElement();
            }
            else
            {
                context.NotifyChunk();
            }
        }
    }
}
