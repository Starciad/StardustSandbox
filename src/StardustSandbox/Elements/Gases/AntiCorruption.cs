using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;
using StardustSandbox.Extensions;
using StardustSandbox.Randomness;
using StardustSandbox.World;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Gases
{
    internal sealed class AntiCorruption : Gas
    {
        private static readonly List<Slot> cachedCorruptionNeighborSlots = [];

        protected override void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors)
        {
            cachedCorruptionNeighborSlots.Clear();

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.HasNeighbor(i))
                {
                    continue;
                }

                Slot slot = neighbors.GetSlot(i);
                SlotLayer slotLayer = slot.GetLayer(context.Layer);

                if (!slotLayer.HasState(ElementStates.IsEmpty) && slotLayer.Element.Characteristics.HasFlag(ElementCharacteristics.IsCorruption))
                {
                    cachedCorruptionNeighborSlots.Add(slot);
                }
            }

            if (cachedCorruptionNeighborSlots.Count > 0)
            {
                Slot corruptionNeighborSlot = cachedCorruptionNeighborSlots.GetRandomItem();
                SlotLayer neighborCorruptionSlotLayer = corruptionNeighborSlot.GetLayer(context.Layer);

                Element currentStoredElement = context.SlotLayer.StoredElement;
                Element neighborStoredElement = neighborCorruptionSlotLayer.StoredElement;

                Point oldPosition = context.Slot.Position;
                Point newPosition = corruptionNeighborSlot.Position;

                if (currentStoredElement == null)
                {
                    context.SwappingElements(newPosition);
                    context.ReplaceElement(oldPosition, ElementIndex.AntiCorruption);
                }
                else
                {
                    context.SwappingElements(newPosition);
                    context.ReplaceElement(oldPosition, currentStoredElement);
                }

                context.SetStoredElement(newPosition, neighborStoredElement);
            }
            else
            {
                if (SSRandom.Chance(15))
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
}
