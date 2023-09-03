using PixelDust.Core.Elements;
using PixelDust.Core.Mathematics;
using PixelDust.Core.Utilities;
using PixelDust.Core.Worlding;
using PixelDust.Game.Elements.Solid.Immovable;

using System;
using System.Collections.Generic;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister(9)]
    internal sealed class Corruption : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Corruption";
            Description = string.Empty;

            Render.AddFrame(new(8, 0));

            EnableNeighborsAction = true;
        }

        protected override void OnNeighbors((Vector2Int, PWorldElementSlot)[] neighbors, int length)
        {
            if (PRandom.Range(0, 300) != 0)
                return;

            List<(Vector2Int, PWorldElementSlot)> targets = new();
            for (int i = 0; i < length; i++)
            {
                if (neighbors[i].Item2.Instance is not Corruption &&
                    neighbors[i].Item2.Instance is not Wall)
                {
                    targets.Add(neighbors[i]);
                }
            }

            if (targets.Count == 0)
                return;

            (Vector2Int, PWorldElementSlot) target = targets.Count == 0 ? targets[0] : targets[PRandom.Range(0, targets.Count)];

            if (target.Item2.Instance is PSolid)
            {
                Context.TryReplace<Corruption>(target.Item1);
            }
            else if (target.Item2.Instance is PLiquid)
            {
                Context.TryReplace<Corruption>(target.Item1);
            }
            else if (target.Item2.Instance is PGas)
            {
                Context.TryReplace<Corruption>(target.Item1);
            }
        }
    }
}