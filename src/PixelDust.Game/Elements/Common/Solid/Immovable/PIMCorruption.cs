using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Common.Utilities;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Elements.Templates.Solid;
using Microsoft.Xna.Framework;
using PixelDust.Game.World.Data;

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
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_18");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_18");

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
