using Microsoft.Xna.Framework;

using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.Items;
using PixelDust.Game.Elements.Common.Liquid;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Items;
using PixelDust.Game.World.Data;

using System;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PGameContent]
    [PElementRegister(0)]
    [PItemRegister(typeof(PDirtItem))]
    public sealed class PDirt : PMovableSolid
    {
        private sealed class PDirtItem : PItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_DIRT";
                this.Name = "Dirt";
                this.Description = string.Empty;
                this.Category = "Powders";
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_1");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(PDirt);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.Game.AssetDatabase.GetTexture("element_1");
            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());
            this.DefaultTemperature = 20;
            this.EnableNeighborsAction = true;
        }
    }
}
