using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Elements.Templates.Solid;

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
            this.Texture = this.Game.AssetDatabase.GetTexture("element_1");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_7");

            this.Rendering.SetRenderingMechanism(new PElementSingleRenderingMechanism());

            this.DefaultTemperature = 22;
        }
    }
}
