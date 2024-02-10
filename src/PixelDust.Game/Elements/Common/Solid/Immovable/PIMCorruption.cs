using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Common.Utilities;
using PixelDust.Game.Elements.Templates.Solid;
using PixelDust.Game.Mathematics;
using PixelDust.Game.Worlding.World.Slots;

using System;

namespace PixelDust.Game.Elements.Common.Solid.Immovable
{
    [PElementRegister(17)]
    public class PIMCorruption : PImmovableSolid
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
            if (this.Context.TryGetElementNeighbors(out ReadOnlySpan<(Vector2Int, PWorldElementSlot)> neighbors))
            {
                this.Context.InfectNeighboringElements(neighbors, neighbors.Length);
            }
        }
    }
}
