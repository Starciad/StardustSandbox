using Microsoft.Xna.Framework;

using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Elements.Common.Utilities;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.World.Data;

using System;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PElementRegister(8)]
    public sealed class PMCorruption : PMovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Corruption (Movable)";
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_9");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_9");

            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());

            this.EnableNeighborsAction = true;
        }

        protected override void OnStep()
        {
            if (this.Context.TryGetElementNeighbors(out ReadOnlySpan<(Point, PWorldSlot)> neighbors))
            {
                this.Context.InfectNeighboringElements(neighbors, neighbors.Length);
            }
        }
    }
}