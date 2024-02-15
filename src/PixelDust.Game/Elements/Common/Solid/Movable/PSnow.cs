using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Elements.Templates.Solid;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PElementRegister(7)]
    public sealed class PSnow : PMovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Snow";
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_1");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_8");

            this.Rendering.SetRenderingMechanism(new PElementSingleRenderingMechanism());

            this.DefaultTemperature = -5;
        }
    }
}
