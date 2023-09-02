using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;
using PixelDust.Core.Mathematics;
using PixelDust.Core.Utilities;
using PixelDust.Core.Worlding;
using PixelDust.Game.Elements.Solid.Immovable;

using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister(9)]
    internal sealed class Corruption : PMovableSolid
    {
        private static bool canInfect = false;

        private static int infectionDelay = infectionDelayRange.Start.Value;
        private static int currentInfectionDelay = 0;

        private static readonly Range infectionDelayRange = new(new(500), new(1000));

        protected override void OnSettings()
        {
            Name = "Corruption";
            Description = string.Empty;

            

            EnableNeighborsAction = true;
            Render.AddFrame(new(8, 0));
        }

        protected override void OnUpdate()
        {
            if (canInfect)
                return;

            if (currentInfectionDelay < infectionDelay)
            {
                currentInfectionDelay++;
            }
            else
            {
                canInfect = true;

                currentInfectionDelay = 0;
                infectionDelay = PRandom.Range(infectionDelayRange.Start.Value, infectionDelayRange.End.Value);
            }
        }

        protected override void OnNeighbors((Vector2Int, PWorldSlot)[] neighbors, int length)
        {
            if (!canInfect)
                return;

            List<(Vector2Int, PWorldSlot)> targets = new();
            for (int i = 0; i < length; i++)
            {
                if (neighbors[i].Item2.Element is not Corruption &&
                    neighbors[i].Item2.Element is not Wall)
                {
                    targets.Add(neighbors[i]);
                }
            }

            if (targets.Count == 0)
                return;

            (Vector2Int, PWorldSlot) target = targets.Count == 0 ? targets[0] : targets[PRandom.Range(0, targets.Count)];

            if (target.Item2.Element is PSolid)
            {
                Context.TryReplace<Corruption>(target.Item1);
            }
            else if (target.Item2.Element is PLiquid)
            {
                Context.TryReplace<Corruption>(target.Item1);
            }
            else if (target.Item2.Element is PGas)
            {
                Context.TryReplace<Corruption>(target.Item1);
            }

            canInfect = false;
        }
    }
}