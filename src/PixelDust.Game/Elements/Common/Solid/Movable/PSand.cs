using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Elements.Rendering.Common;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PElementRegister(6)]
    public sealed class PSand : PMovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Sand";
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_7");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_7");

            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());

            this.DefaultTemperature = 22;
        }
    }
}
