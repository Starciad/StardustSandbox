using StardustSandbox.ContentBundle.Elements.Gases;
using StardustSandbox.ContentBundle.Elements.Liquids;
using StardustSandbox.ContentBundle.Elements.Solids.Immovables;
using StardustSandbox.ContentBundle.Elements.Solids.Movables;
using StardustSandbox.Core.Elements.Templates.Gases;
using StardustSandbox.Core.Elements.Templates.Liquids;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces.Elements;
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

        internal static bool CheckIfNeighboringElementsAreCorrupted(SWorldLayer worldLayer, SWorldSlot[] neighbors, int length)
        {
            if (length == 0)
            {
                return true;
            }

            int corruptNeighboringElements = 0;

            for (int i = 0; i < length; i++)
            {
                SWorldSlot worldSlot = neighbors[i];

                if (worldSlot.GetLayer(worldLayer).Element is ISCorruption)
                {
                    corruptNeighboringElements++;
                }
            }

            return corruptNeighboringElements == length;
        }

        internal static void InfectNeighboringElements(this ISElementContext context, SWorldSlot[] neighbors, int length)
        {
            if (length == 0)
            {
                return;
            }

            targets.Clear();

            for (int i = 0; i < length; i++)
            {
                SWorldSlot slot = neighbors[i];

                ISElement foregroundElement = slot.ForegroundLayer.Element;
                if (foregroundElement is not ISCorruption && foregroundElement is not SWall)
                {
                    targets.Add(new(slot, SWorldLayer.Foreground));
                }

                ISElement backgroundElement = slot.BackgroundLayer.Element;
                if (backgroundElement is not ISCorruption && backgroundElement is not SWall)
                {
                    targets.Add(new(slot, SWorldLayer.Background));
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
                    context.ReplaceElement<SMCorruption>(slotTarget.Slot.Position, slotTarget.Layer);
                    break;

                case SImmovableSolid:
                    context.ReplaceElement<SIMCorruption>(slotTarget.Slot.Position, slotTarget.Layer);
                    break;

                case SLiquid:
                    context.ReplaceElement<SLCorruption>(slotTarget.Slot.Position, slotTarget.Layer);
                    break;

                case SGas:
                    context.ReplaceElement<SGCorruption>(slotTarget.Slot.Position, slotTarget.Layer);
                    break;

                default:
                    context.ReplaceElement<SMCorruption>(slotTarget.Slot.Position, slotTarget.Layer);
                    break;
            }
        }
    }
}