using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Extensions;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Utilities
{
    internal static class CorruptionUtilities
    {
        private readonly struct SSlotTarget(Slot slot, LayerType layer)
        {
            internal Slot Slot => slot;
            internal LayerType Layer => layer;
        }

        private static readonly List<SSlotTarget> targets = [];

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

                if (element.HasCharacteristic(ElementCharacteristics.IsCorruption))
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
                if (element != null && element.HasCharacteristic(ElementCharacteristics.IsCorruptible))
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

        private static void InfectSlotLayer(ElementContext context, SSlotTarget slotTarget)
        {
            Element targetElement = slotTarget.Layer == LayerType.Foreground
                ? slotTarget.Slot.ForegroundLayer.Element
                : slotTarget.Slot.BackgroundLayer.Element;

            switch (targetElement.Category)
            {
                case ElementCategory.MovableSolid:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, ElementIndex.MCorruption);
                    break;

                case ElementCategory.ImmovableSolid:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, ElementIndex.IMCorruption);
                    break;

                case ElementCategory.Liquid:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, ElementIndex.LCorruption);
                    break;

                case ElementCategory.Gas:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, ElementIndex.GCorruption);
                    break;

                case ElementCategory.None:
                case ElementCategory.Energy:
                default:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, ElementIndex.MCorruption);
                    break;
            }
        }
    }
}