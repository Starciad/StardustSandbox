using PixelDust.Core.Elements;
using PixelDust.Core.Worlding;
using PixelDust.Mathematics;
using PixelDust.Game.Elements.Helpers;
using System;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister(9)]
    internal sealed class MCorruption : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Corruption (Movable)";
            Description = string.Empty;

            Render.AddFrame(new(8, 0));

            EnableNeighborsAction = true;
        }

        protected override void OnStep()
        {
            if (Context.TryGetNeighbors(Context.Position, out ReadOnlySpan<(Vector2Int, PWorldElementSlot)> neighbors))
            {
                Context.InfectNeighboringElements(neighbors, neighbors.Length);
            }
        }
    }
}