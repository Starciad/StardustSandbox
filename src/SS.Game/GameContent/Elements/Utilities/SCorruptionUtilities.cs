using Microsoft.Xna.Framework;

using StardustSandbox.Game.Elements;
using StardustSandbox.Game.Elements.Templates.Gases;
using StardustSandbox.Game.Elements.Templates.Liquids;
using StardustSandbox.Game.Elements.Templates.Solids.Immovables;
using StardustSandbox.Game.Elements.Templates.Solids.Movables;
using StardustSandbox.Game.GameContent.Elements.Gases;
using StardustSandbox.Game.GameContent.Elements.Liquids;
using StardustSandbox.Game.GameContent.Elements.Solids.Immovables;
using StardustSandbox.Game.GameContent.Elements.Solids.Movables;
using StardustSandbox.Game.Interfaces.Elements;
using StardustSandbox.Game.Interfaces.Elements.Templates;
using StardustSandbox.Game.Mathematics;
using StardustSandbox.Game.World.Data;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Game.GameContent.Elements.Utilities
{
    public static class SCorruptionUtilities
    {
        public static bool CheckIfNeighboringElementsAreCorrupted(this ISElementContext context, ReadOnlySpan<(Point, SWorldSlot)> neighbors, int length)
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

            if (corruptNeighboringElements == length)
            {
                return true;
            }

            return false;
        }

        public static void InfectNeighboringElements(this ISElementContext context, ReadOnlySpan<(Point, SWorldSlot)> neighbors, int length)
        {
            if (length == 0)
            {
                return;
            }

            List<(Point, SWorldSlot)> targets = [];
            for (int i = 0; i < length; i++)
            {
                SElement element = neighbors[i].Item2.Element;

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

            (Point, SWorldSlot) target = targets.Count == 0 ? targets[0] : targets[SRandomMath.Range(0, targets.Count)];
            SElement targetElement = target.Item2.Element;

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