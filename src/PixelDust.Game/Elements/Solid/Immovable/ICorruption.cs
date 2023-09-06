using PixelDust.Core.Elements;
using PixelDust.Core.Mathematics;
using PixelDust.Core.Worlding;
using PixelDust.Game.Elements.Helpers;

using System;

namespace PixelDust.Game.Elements.Solid.Immovable
{
    [PElementRegister(18)]
    internal class ICorruption : PImmovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Corruption (Immovable)";
            Description = string.Empty;

            Render.AddFrame(new(7, 1));

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
