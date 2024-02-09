using PixelDust.Core.Elements.Attributes;
using PixelDust.Core.Elements.Templates.Solid;
using PixelDust.Core.Worlding.World.Slots;
using PixelDust.Game.Elements.Utilities;
using PixelDust.Mathematics;

using System;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister(9)]
    internal sealed class MCorruption : PMovableSolid
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