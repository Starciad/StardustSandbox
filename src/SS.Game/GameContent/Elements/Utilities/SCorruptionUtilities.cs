using Microsoft.Xna.Framework;

using StardustSandbox.Game.Elements;
using StardustSandbox.Game.Elements.Contexts;
using StardustSandbox.Game.Elements.Templates.Gases;
using StardustSandbox.Game.Elements.Templates.Liquids;
using StardustSandbox.Game.Elements.Templates.Solids;
using StardustSandbox.Game.Elements.Templates.Solids.Immovables;
using StardustSandbox.Game.GameContent.Elements.Gases;
using StardustSandbox.Game.GameContent.Elements.Liquids;
using StardustSandbox.Game.GameContent.Elements.Solids.Immovables;
using StardustSandbox.Game.GameContent.Elements.Solids.Movables;
using StardustSandbox.Game.Mathematics;
using StardustSandbox.Game.World.Data;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Game.GameContent.Elements.Utilities
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

            (Point, SWorldSlot) target = targets.Count == 0 ? targets[0] : targets[SRandomMath.Range(0, targets.Count)];
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