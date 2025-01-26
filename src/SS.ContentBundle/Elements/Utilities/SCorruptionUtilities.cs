using StardustSandbox.ContentBundle.Elements.Solids.Immovables;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Templates.Gases;
using StardustSandbox.Core.Elements.Templates.Liquids;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.Elements.Contexts;
using StardustSandbox.Core.Interfaces.Elements.Templates;
using StardustSandbox.Core.World.Slots;

using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.Elements.Utilities
{
    internal static class SCorruptionUtilities
    {
        private readonly struct SSlotTarget(SWorldSlot slot, SWorldLayer layer)
        {
            public SWorldSlot Slot => slot;
            public SWorldLayer Layer => layer;
        }

        private static readonly List<SSlotTarget> targets = [];

        internal static bool CheckIfNeighboringElementsAreCorrupted(SWorldLayer worldLayer, IEnumerable<SWorldSlot> neighbors)
        {
            int count = 0;
            int corruptNeighboringElements = 0;

            foreach (SWorldSlot neighbor in neighbors)
            {
                if (neighbor.GetLayer(worldLayer).Element is ISCorruption)
                {
                    corruptNeighboringElements++;
                }

                count++;
            }

            return corruptNeighboringElements == count;
        }

        internal static void InfectNeighboringElements(this ISElementContext context, IEnumerable<SWorldSlot> neighbors)
        {
            targets.Clear();

            void ProcessLayer(SWorldSlot slot, SWorldLayer layer, ISElement element)
            {
                if (element is not ISCorruption && element is not (SWall or SClone))
                {
                    targets.Add(new(slot, layer));
                }
            }

            foreach (SWorldSlot neighbor in neighbors)
            {
                ProcessLayer(neighbor, SWorldLayer.Foreground, neighbor.ForegroundLayer.Element);
                ProcessLayer(neighbor, SWorldLayer.Background, neighbor.BackgroundLayer.Element);
            }

            if (targets.Count == 0)
            {
                return;
            }

            InfectWorldSlotLayer(context, targets.GetRandomItem());
        }

        private static void InfectWorldSlotLayer(ISElementContext context, SSlotTarget slotTarget)
        {
            ISElement targetElement = slotTarget.Layer == SWorldLayer.Foreground
                ? slotTarget.Slot.ForegroundLayer.Element
                : slotTarget.Slot.BackgroundLayer.Element;

            switch (targetElement)
            {
                case SMovableSolid:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, SElementConstants.MOVABLE_CORRUPTION_IDENTIFIER);
                    break;

                case SImmovableSolid:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, SElementConstants.IMMOVABLE_CORRUPTION_IDENTIFIER);
                    break;

                case SLiquid:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, SElementConstants.LIQUID_CORRUPTION_IDENTIFIER);
                    break;

                case SGas:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, SElementConstants.GAS_CORRUPTION_IDENTIFIER);
                    break;

                default:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, SElementConstants.MOVABLE_CORRUPTION_IDENTIFIER);
                    break;
            }
        }
    }
}