using Microsoft.Xna.Framework;

using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.Items;
using PixelDust.Game.Elements.Common.Utilities;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Items;
using PixelDust.Game.World.Data;

using System;

namespace PixelDust.Game.Elements.Common.Solid.Immovable
{
    [PGameContent]
    [PElementRegister(17)]
    [PItemRegister(typeof(PIMCorruptionItem))]
    public class PIMCorruption : PImmovableSolid
    {
        private sealed class PIMCorruptionItem : PItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_CORRUPTION_IMMOVABLE";
                this.Name = "Corruption (Immovable)";
                this.Description = string.Empty;
                this.Category = string.Empty;
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_18");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(PIMCorruption);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.Game.AssetDatabase.GetTexture("element_18");
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
