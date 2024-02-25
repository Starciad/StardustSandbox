using Microsoft.Xna.Framework;

using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.Items;
using PixelDust.Game.Elements.Common.Utilities;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Items;
using PixelDust.Game.World.Data;

using System;

namespace PixelDust.Game.Elements.Common.Liquid
{
    [PGameContent]
    [PElementRegister(16)]
    [PItemRegister(typeof(PLCorruptionItem))]
    public class PLCorruption : PLiquid
    {
        private sealed class PLCorruptionItem : PItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_CORRUPTION_LIQUID";
                this.Name = "Corruption (Liquid)";
                this.Description = string.Empty;
                this.Category = string.Empty;
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_17");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(PLCorruption);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.Game.AssetDatabase.GetTexture("element_17");
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
