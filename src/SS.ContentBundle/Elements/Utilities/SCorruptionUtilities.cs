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
using StardustSandbox.Core.Interfaces.World;

using System;
using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.Elements.Utilities
{
    internal static class SCorruptionUtilities
    {
        private static readonly List<(ISWorldSlot slot, SWorldLayer layer)> targets = [];

        internal static bool CheckIfNeighboringElementsAreCorrupted(SWorldLayer worldLayer, ReadOnlySpan<ISWorldSlot> neighbors, int length)
        {
            if (length == 0)
            {
                return true;
            }

            int corruptNeighboringElements = 0;

            for (int i = 0; i < length; i++)
            {
                ISWorldSlot worldSlot = neighbors[i];

                if (worldSlot.GetLayer(worldLayer).Element is ISCorruption)
                {
                    corruptNeighboringElements++;
                }
            }

            return corruptNeighboringElements == length;
        }

        internal static void InfectNeighboringElements(this ISElementContext context, ReadOnlySpan<ISWorldSlot> neighbors, int length)
        {
            if (length == 0)
            {
                return;
            }

            targets.Clear();

            for (int i = 0; i < length; i++)
            {
                ISWorldSlot slot = neighbors[i];

                ISElement foregroundElement = slot.ForegroundLayer.Element;
                if (foregroundElement is not ISCorruption && foregroundElement is not SWall)
                {
                    targets.Add((slot, SWorldLayer.Foreground));
                }

                ISElement backgroundElement = slot.BackgroundLayer.Element;
                if (backgroundElement is not ISCorruption && backgroundElement is not SWall)
                {
                    targets.Add((slot, SWorldLayer.Background));
                }
            }

            if (targets.Count == 0)
            {
                return;
            }

            (ISWorldSlot targetSlot, SWorldLayer targetLayer) = targets.GetRandomItem();

            InfectWorldSlotLayer(context, targetSlot, targetLayer);
        }

        private static void InfectWorldSlotLayer(ISElementContext context, ISWorldSlot worldSlot, SWorldLayer targetLayer)
        {
            ISElement targetElement = targetLayer == SWorldLayer.Foreground
                ? worldSlot.ForegroundLayer.Element
                : worldSlot.BackgroundLayer.Element;

            switch (targetElement)
            {
                case SMovableSolid:
                    context.ReplaceElement<SMCorruption>(worldSlot.Position, targetLayer);
                    break;

                case SImmovableSolid:
                    context.ReplaceElement<SIMCorruption>(worldSlot.Position, targetLayer);
                    break;

                case SLiquid:
                    context.ReplaceElement<SLCorruption>(worldSlot.Position, targetLayer);
                    break;

                case SGas:
                    context.ReplaceElement<SGCorruption>(worldSlot.Position, targetLayer);
                    break;

                default:
                    context.ReplaceElement<SMCorruption>(worldSlot.Position, targetLayer);
                    break;
            }
        }
    }
}