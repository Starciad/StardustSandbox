using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Common.Utilities;
using PixelDust.Game.Elements.Templates.Gases;
using PixelDust.Game.Mathematics;
using PixelDust.Game.Worlding.World.Slots;

using System;

namespace PixelDust.Game.Elements.Common.Gases
{
    [PElementRegister(16)]
    public class GCorruption : PGas
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
            if (this.Context.TryGetElementNeighbors(out ReadOnlySpan<(Vector2Int, PWorldElementSlot)> neighbors))
            {
                this.Context.InfectNeighboringElements(neighbors, neighbors.Length);
            }
        }
    }
}
