using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Elements.Templates.Solid;

using Microsoft.Xna.Framework;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PElementRegister(0)]
    public sealed class PDirt : PMovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Dirt";
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_1");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_1");

            this.Rendering.SetRenderingMechanism(new PElementBlobRendering());

            this.DefaultTemperature = 20;
        }
    }
}
