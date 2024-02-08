using PixelDust.Core.Elements.Context;
using PixelDust.Core.Elements.Types.Gases;
using PixelDust.Core.Elements.Types.Liquid;
using PixelDust.Core.Elements.Types.Solid;
using PixelDust.Core.Utilities;
using PixelDust.Core.Worlding.World.Slots;
using PixelDust.Game.Elements.Gases;
using PixelDust.Game.Elements.Liquid;
using PixelDust.Game.Elements.Solid.Immovable;
using PixelDust.Game.Elements.Solid.Movable;
using PixelDust.Mathematics;

using System;
using System.Collections.Generic;

namespace PixelDust.Game.Elements.Utilities
{
    internal static class CorruptionUtilities
    {
        public static void InfectNeighboringElements(this PElementContext context, ReadOnlySpan<(Vector2Int, PWorldElementSlot)> neighbors, int length)
        {
            List<(Vector2Int, PWorldElementSlot)> targets = [];
            for (int i = 0; i < length; i++)
            {
                if (neighbors[i].Item2.Instance is not MCorruption &&
                    neighbors[i].Item2.Instance is not ICorruption &&
                    neighbors[i].Item2.Instance is not LCorruption &&
                    neighbors[i].Item2.Instance is not GCorruption &&
                    neighbors[i].Item2.Instance is not Wall)
                {
                    targets.Add(neighbors[i]);
                }
            }

            if (targets.Count == 0)
            {
                return;
            }

            (Vector2Int, PWorldElementSlot) target = targets.Count == 0 ? targets[0] : targets[PRandom.Range(0, targets.Count)];

            if (target.Item2.Instance is PSolid)
            {
                _ = context.TryReplace<MCorruption>(target.Item1);
            }
            else if (target.Item2.Instance is PImmovableSolid)
            {
                _ = context.TryReplace<ICorruption>(target.Item1);
            }
            else if (target.Item2.Instance is PLiquid)
            {
                _ = context.TryReplace<LCorruption>(target.Item1);
            }
            else if (target.Item2.Instance is PGas)
            {
                _ = context.TryReplace<GCorruption>(target.Item1);
            }
        }
    }
}