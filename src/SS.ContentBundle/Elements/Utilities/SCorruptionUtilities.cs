using StardustSandbox.ContentBundle.Elements.Gases;
using StardustSandbox.ContentBundle.Elements.Liquids;
using StardustSandbox.ContentBundle.Elements.Solids.Immovables;
using StardustSandbox.ContentBundle.Elements.Solids.Movables;
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
using StardustSandbox.Core.World.Data;

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

            foreach (SWorldSlot neighbor in neighbors)
            {
                ISElement foregroundElement = neighbor.ForegroundLayer.Element;
                if (foregroundElement is not ISCorruption && foregroundElement is not SWall)
                {
                    targets.Add(new(neighbor, SWorldLayer.Foreground));
                }

                ISElement backgroundElement = neighbor.BackgroundLayer.Element;
                if (backgroundElement is not ISCorruption && backgroundElement is not SWall)
                {
                    targets.Add(new(neighbor, SWorldLayer.Background));
                }
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
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, SElementIdentifierConstants.MOVABLE_CORRUPTION);
                    break;

                case SImmovableSolid:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, SElementIdentifierConstants.IMMOVABLE_CORRUPTION);
                    break;

                case SLiquid:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, SElementIdentifierConstants.LIQUID_CORRUPTION);
                    break;

                case SGas:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, SElementIdentifierConstants.GAS_CORRUPTION);
                    break;

                default:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, SElementIdentifierConstants.MOVABLE_CORRUPTION);
                    break;
            }
        }
    }
}