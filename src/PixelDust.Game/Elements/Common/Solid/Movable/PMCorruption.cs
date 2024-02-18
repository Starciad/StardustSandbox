using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Common.Utilities;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Elements.Templates.Solid;
using PixelDust.Game.Mathematics;
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
            if (this.Context.TryGetElementNeighbors(out ReadOnlySpan<(Vector2Int, PWorldSlot)> neighbors))
            {
                this.Context.InfectNeighboringElements(neighbors, neighbors.Length);
            }
        }
    }
}