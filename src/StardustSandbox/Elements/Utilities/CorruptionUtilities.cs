using StardustSandbox.Elements.Gases;
using StardustSandbox.Elements.Liquids;
using StardustSandbox.Elements.Solids.Immovables;
using StardustSandbox.Elements.Solids.Movables;
using StardustSandbox.Enums.Indexers;
using StardustSandbox.Enums.World;
using StardustSandbox.Extensions;
using StardustSandbox.Interfaces.Elements;
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

        internal static bool CheckIfNeighboringElementsAreCorrupted(LayerType worldLayer, IEnumerable<Slot> neighbors)
        {
            int count = 0;
            int corruptNeighboringElements = 0;

            foreach (Slot neighbor in neighbors)
            {
                if (neighbor.GetLayer(worldLayer).Element is ICorruptionElement)
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
                if (element is not ICorruptionElement && element is not (Wall or Clone))
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

            switch (targetElement)
            {
                case MovableSolid:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, ElementIndex.MCorruption);
                    break;

                case ImmovableSolid:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, ElementIndex.IMCorruption);
                    break;

                case Liquid:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, ElementIndex.LCorruption);
                    break;

                case Gas:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, ElementIndex.GCorruption);
                    break;

                default:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, ElementIndex.MCorruption);
                    break;
            }
        }
    }
}