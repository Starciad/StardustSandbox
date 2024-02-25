using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Elements.Rendering.Common;

namespace PixelDust.Game.Elements.Common.Solid.Immovable
{
    [PElementRegister(13)]
    public sealed class PWall : PImmovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Wall";
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_14");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_14");

            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());

            this.EnableTemperature = false;
        }
    }
}
