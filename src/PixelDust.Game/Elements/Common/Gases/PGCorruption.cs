using Microsoft.Xna.Framework;

using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Common.Utilities;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Elements.Templates.Gases;
using PixelDust.Game.World.Data;

using System;

namespace PixelDust.Game.Elements.Common.Gases
{
    [PElementRegister(15)]
    public class PGCorruption : PGas
    {
        protected override void OnSettings()
        {
            this.Name = "Corruption (Gas)";
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_16");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_16");

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
