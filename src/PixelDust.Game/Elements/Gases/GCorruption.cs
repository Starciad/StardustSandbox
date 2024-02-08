using PixelDust.Core.Elements.Attributes;
using PixelDust.Core.Elements.Types.Gases;
using PixelDust.Core.Worlding.World.Slots;
using PixelDust.Game.Elements.Utilities;
using PixelDust.Mathematics;

using System;

namespace PixelDust.Game.Elements.Gases
{
    [PElementRegister(16)]
    internal class GCorruption : PGas
    {
        protected override void OnSettings()
        {
            this.Name = "Corruption (Gas)";
            this.Description = string.Empty;

            this.Render.AddFrame(new(5, 1));

            this.EnableNeighborsAction = true;
        }

        protected override void OnStep()
        {
            if (this.Context.TryGetNeighbors(this.Context.Position, out ReadOnlySpan<(Vector2Int, PWorldElementSlot)> neighbors))
            {
                this.Context.InfectNeighboringElements(neighbors, neighbors.Length);
            }
        }
    }
}
