using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Elements.Rendering.Common;

namespace PixelDust.Game.Elements.Common.Solid.Immovable
{
    [PElementRegister(14)]
    public sealed class PWood : PImmovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Wood";
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_15");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_15");

            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());

            this.DefaultTemperature = 20;
        }
    }
}
