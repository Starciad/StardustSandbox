using PixelDust.Core.Elements;
using PixelDust.Core.Worlding;
using PixelDust.Game.Elements.Helpers;
using PixelDust.Mathematics;

using System;

namespace PixelDust.Game.Elements.Gases
{
    [PElementRegister(16)]
    internal class GCorruption : PGas
    {
        protected override void OnSettings()
        {
            Name = "Corruption (Gas)";
            Description = string.Empty;

            Render.AddFrame(new(5, 1));

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
