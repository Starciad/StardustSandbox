using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.Elements.Gases;
using StardustSandbox.ContentBundle.Elements.Liquids;
using StardustSandbox.ContentBundle.Elements.Solids.Immovables;
using StardustSandbox.ContentBundle.Elements.Solids.Movables;
using StardustSandbox.Core.Elements.Templates.Gases;
using StardustSandbox.Core.Elements.Templates.Liquids;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.Elements.Templates;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Mathematics;

using System;
using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.Elements.Utilities
{
    public static class SCorruptionUtilities
    {
        public static bool CheckIfNeighboringElementsAreCorrupted(this ISElementContext context, ReadOnlySpan<(Point, ISWorldSlot)> neighbors, int length)
        {
            if (length == 0)
            {
                return true;
            }

            int corruptNeighboringElements = 0;

            for (int i = 0; i < length; i++)
            {
                if (neighbors[i].Item2.Element is ISCorruption)
                {
                    corruptNeighboringElements++;
                }
            }

            return corruptNeighboringElements == length;
        }

        public static void InfectNeighboringElements(this ISElementContext context, ReadOnlySpan<(Point, ISWorldSlot)> neighbors, int length)
        {
            if (length == 0)
            {
                return;
            }

            List<(Point, ISWorldSlot)> targets = [];
            for (int i = 0; i < length; i++)
            {
                ISElement element = neighbors[i].Item2.Element;

                if (element is not ISCorruption &&
                    element is not SWall)
                {
                    targets.Add(neighbors[i]);
                }
            }

            if (targets.Count == 0)
            {
                return;
            }

            (Point, ISWorldSlot) target = targets.Count == 0 ? targets[0] : targets[SRandomMath.Range(0, targets.Count)];
            ISElement targetElement = target.Item2.Element;

            if (targetElement is SMovableSolid)
            {
                context.ReplaceElement<SMCorruption>(target.Item1);
            }
            else if (targetElement is SImmovableSolid)
            {
                context.ReplaceElement<SIMCorruption>(target.Item1);
            }
            else if (targetElement is SLiquid)
            {
                context.ReplaceElement<SLCorruption>(target.Item1);
            }
            else if (targetElement is SGas)
            {
                context.ReplaceElement<SGCorruption>(target.Item1);
            }
            else
            {
                context.ReplaceElement<SMCorruption>(target.Item1);
            }
        }
    }
}