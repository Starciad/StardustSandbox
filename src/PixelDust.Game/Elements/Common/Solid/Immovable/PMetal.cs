using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Elements.Rendering.Common;

namespace PixelDust.Game.Elements.Common.Solid.Immovable
{
    [PElementRegister(12)]
    public sealed class PMetal : PImmovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Metal";
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_13");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_13");

            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());

            this.DefaultTemperature = 30;
        }
    }
}
