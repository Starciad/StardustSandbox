using PixelDust.Core.Elements.Attributes;
using PixelDust.Core.Elements.Types.Solid;
using PixelDust.Core.Worlding.World.Slots;
using PixelDust.Game.Elements.Utilities;
using PixelDust.Mathematics;

using System;

namespace PixelDust.Game.Elements.Solid.Immovable
{
    [PElementRegister(18)]
    internal class ICorruption : PImmovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Corruption (Immovable)";
            this.Description = string.Empty;

            this.Render.AddFrame(new(7, 1));

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
