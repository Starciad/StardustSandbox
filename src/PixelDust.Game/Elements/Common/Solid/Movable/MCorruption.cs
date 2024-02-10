using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Common.Utilities;
using PixelDust.Game.Elements.Templates.Solid;
using PixelDust.Game.Mathematics;
using PixelDust.Game.Worlding.World.Slots;

using System;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PElementRegister(9)]
    public sealed class MCorruption : PMovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Corruption (Movable)";
            this.Description = string.Empty;

            this.Render.AddFrame(new(8, 0));

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