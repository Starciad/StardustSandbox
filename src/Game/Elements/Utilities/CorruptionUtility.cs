using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Extensions;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Utilities
{
    internal static class CorruptionUtility
    {
        private readonly struct SlotTarget(Slot slot, Layer layer)
        {
            internal readonly Slot Slot => slot;
            internal readonly Layer Layer => layer;
        }

        private static readonly List<SlotTarget> targets = [];

        internal static bool CheckIfNeighboringElementsAreCorrupted(in Layer layer, in ElementNeighbors neighbors)
        {
            int count = 0;
            int corruptNeighboringElements = 0;

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.HasNeighbor(i))
                {
                    continue;
                }

                Element element = neighbors.GetSlotLayer(i, layer).Element;

                if (element == null)
                {
                    continue;
                }

                if (element.Characteristics.HasFlag(ElementCharacteristics.IsCorruption))
                {
                    corruptNeighboringElements++;
                }

                count++;
            }

            return corruptNeighboringElements == count;
        }

        internal static void InfectNeighboringElements(this ElementContext context, in ElementNeighbors neighbors)
        {
            targets.Clear();

            void ProcessLayer(Slot slot, Layer layer, Element element)
            {
                if (element.Characteristics.HasFlag(ElementCharacteristics.IsCorruptible))
                {
                    targets.Add(new(slot, layer));
                }
            }

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.HasNeighbor(i))
                {
                    continue;
                }

                if (!neighbors.GetSlot(i).IsForegroundEmpty)
                {
                    ProcessLayer(neighbors.GetSlot(i), Layer.Foreground, neighbors.GetSlotLayer(i, Layer.Foreground).Element);
                }

                if (!neighbors.GetSlot(i).IsBackgroundEmpty)
                {
                    ProcessLayer(neighbors.GetSlot(i), Layer.Background, neighbors.GetSlotLayer(i, Layer.Background).Element);
                }
            }

            if (targets.Count == 0)
            {
                return;
            }

            InfectSlotLayer(context, targets.GetRandomItem());
        }

        private static void InfectSlotLayer(in ElementContext context, in SlotTarget slotTarget)
        {
            Element targetElement = slotTarget.Layer == Layer.Foreground
                ? slotTarget.Slot.Foreground.Element
                : slotTarget.Slot.Background.Element;

            switch (targetElement.Category)
            {
                case ElementCategory.MovableSolid:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, ElementIndex.MovableCorruption);
                    break;

                case ElementCategory.ImmovableSolid:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, ElementIndex.ImmovableCorruption);
                    break;

                case ElementCategory.Liquid:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, ElementIndex.LiquidCorruption);
                    break;

                case ElementCategory.Gas:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, ElementIndex.GasCorruption);
                    break;

                default:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, ElementIndex.MovableCorruption);
                    break;
            }

            context.SetStoredElement(slotTarget.Slot.Position, slotTarget.Layer, targetElement);
        }
    }
}