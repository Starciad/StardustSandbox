using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Common.Utilities;
using PixelDust.Game.Elements.Templates.Liquid;
using PixelDust.Game.Mathematics;
using PixelDust.Game.World.Slots;

using System;

namespace PixelDust.Game.Elements.Common.Liquid
{
    [PElementRegister(16)]
    public class PLCorruption : PLiquid
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
