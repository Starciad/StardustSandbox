using PixelDust.Game.Elements.Common.Gases;
using PixelDust.Game.Elements.Common.Liquid;
using PixelDust.Game.Elements.Common.Solid.Immovable;
using PixelDust.Game.Elements.Common.Solid.Movable;
using PixelDust.Game.Elements.Contexts;
using PixelDust.Game.Elements.Templates.Gases;
using PixelDust.Game.Elements.Templates.Liquid;
using PixelDust.Game.Elements.Templates.Solid;
using PixelDust.Game.Mathematics;
using PixelDust.Game.Utilities;
using PixelDust.Game.World.Slots;

using System;
using System.Collections.Generic;

namespace PixelDust.Game.Elements.Common.Utilities
{
    public static class PCorruptionUtilities
    {
        public static void InfectNeighboringElements(this PElementContext context, ReadOnlySpan<(Vector2Int, PWorldElementSlot)> neighbors, int length)
        {
            List<(Vector2Int, PWorldElementSlot)> targets = [];
            for (int i = 0; i < length; i++)
            {
                PElement element = context.ElementDatabase.GetElementById(neighbors[i].Item2.Id);

                if (element is not PMCorruption &&
                    element is not PIMCorruption &&
                    element is not PLCorruption &&
                    element is not PGCorruption &&
                    element is not PWall)
                {
                    targets.Add(neighbors[i]);
                }
            }

            if (targets.Count == 0)
            {
                return;
            }

            (Vector2Int, PWorldElementSlot) target = targets.Count == 0 ? targets[0] : targets[PRandom.Range(0, targets.Count)];
            PElement targetElement = context.ElementDatabase.GetElementById(target.Item2.Id);

            if (targetElement is PSolid)
            {
                context.ReplaceElement<PMCorruption>(target.Item1);
            }
            else if (targetElement is PImmovableSolid)
            {
                context.ReplaceElement<PIMCorruption>(target.Item1);
            }
            else if (targetElement is PLiquid)
            {
                context.ReplaceElement<PLCorruption>(target.Item1);
            }
            else if (targetElement is PGas)
            {
                context.ReplaceElement<PGCorruption>(target.Item1);
            }
            else
            {
                context.ReplaceElement<PMCorruption>(target.Item1);
            }
        }
    }
}