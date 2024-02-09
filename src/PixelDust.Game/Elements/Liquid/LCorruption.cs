using PixelDust.Core.Elements.Attributes;
using PixelDust.Core.Elements.Templates.Liquid;
using PixelDust.Core.Worlding.World.Slots;
using PixelDust.Game.Elements.Utilities;
using PixelDust.Mathematics;

using System;

namespace PixelDust.Game.Elements.Liquid
{
    [PElementRegister(17)]
    internal class LCorruption : PLiquid
    {
        protected override void OnSettings()
        {
            this.Name = "Corruption (Liquid)";
            this.Description = string.Empty;

            this.Render.AddFrame(new(6, 1));

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
