using PixelDust.Core.Elements;
using PixelDust.Core.Worlding;
using PixelDust.Mathematics;
using PixelDust.Game.Elements.Helpers;
using System;

namespace PixelDust.Game.Elements.Liquid
{
    [PElementRegister(17)]
    internal class LCorruption : PLiquid
    {
        protected override void OnSettings()
        {
            Name = "Corruption (Liquid)";
            Description = string.Empty;

            Render.AddFrame(new(6, 1));

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
