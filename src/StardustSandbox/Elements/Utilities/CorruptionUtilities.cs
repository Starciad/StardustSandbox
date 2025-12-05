using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Extensions;
using StardustSandbox.World;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Utilities
{
    internal static class CorruptionUtilities
    {
        private readonly struct SlotTarget(Slot slot, LayerType layer)
        {
            internal Slot Slot => slot;
            internal LayerType Layer => layer;
        }

        private static readonly List<SlotTarget> targets = [];

        internal static bool CheckIfNeighboringElementsAreCorrupted(LayerType layer, IEnumerable<Slot> neighbors)
        {
            int count = 0;
            int corruptNeighboringElements = 0;

            foreach (Slot neighbor in neighbors)
            {
                Element element = neighbor.GetLayer(layer).Element;

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

        internal static void InfectNeighboringElements(this ElementContext context, IEnumerable<Slot> neighbors)
        {
            targets.Clear();

            void ProcessLayer(Slot slot, LayerType layer, Element element)
            {
                if (element != null && element.Characteristics.HasFlag(ElementCharacteristics.IsCorruptible))
                {
                    targets.Add(new(slot, layer));
                }
            }

            foreach (Slot neighbor in neighbors)
            {
                ProcessLayer(neighbor, LayerType.Foreground, neighbor.ForegroundLayer.Element);
                ProcessLayer(neighbor, LayerType.Background, neighbor.BackgroundLayer.Element);
            }

            if (targets.Count == 0)
            {
                return;
            }

            InfectSlotLayer(context, targets.GetRandomItem());
        }

        private static void InfectSlotLayer(in ElementContext context, SlotTarget slotTarget)
        {
            Element targetElement = slotTarget.Layer == LayerType.Foreground
                ? slotTarget.Slot.ForegroundLayer.Element
                : slotTarget.Slot.BackgroundLayer.Element;

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

                case ElementCategory.None:
                case ElementCategory.Energy:
                default:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, ElementIndex.MovableCorruption);
                    break;
            }
        }
    }
}