using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Elements.Rendering.Common;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PElementRegister(5)]
    public sealed class PIce : PMovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Ice";
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_6");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_6");

            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());

            this.DefaultTemperature = 0;
        }
    }
}
