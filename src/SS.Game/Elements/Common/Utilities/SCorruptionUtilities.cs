using Microsoft.Xna.Framework;

using StardustSandbox.Game.Elements.Common.Gases;
using StardustSandbox.Game.Elements.Common.Liquid;
using StardustSandbox.Game.Elements.Common.Solid;
using StardustSandbox.Game.Elements.Common.Solid.Immovable;
using StardustSandbox.Game.Elements.Common.Solid.Movable;
using StardustSandbox.Game.Elements.Contexts;
using StardustSandbox.Game.General;
using StardustSandbox.Game.World.Data;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Game.Elements.Common.Utilities
{
    public static class SCorruptionUtilities
    {
        public static void InfectNeighboringElements(this SElementContext context, ReadOnlySpan<(Point, SWorldSlot)> neighbors, int length)
        {
            List<(Point, SWorldSlot)> targets = [];
            for (int i = 0; i < length; i++)
            {
                SElement element = context.ElementDatabase.GetElementById(neighbors[i].Item2.Id);

                if (element is not SMCorruption &&
                    element is not SIMCorruption &&
                    element is not SLCorruption &&
                    element is not SGCorruption &&
                    element is not SWall)
                {
                    targets.Add(neighbors[i]);
                }
            }

            if (targets.Count == 0)
            {
                return;
            }

            (Point, SWorldSlot) target = targets.Count == 0 ? targets[0] : targets[SRandom.Range(0, targets.Count)];
            SElement targetElement = context.ElementDatabase.GetElementById(target.Item2.Id);

            if (targetElement is SSolid)
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