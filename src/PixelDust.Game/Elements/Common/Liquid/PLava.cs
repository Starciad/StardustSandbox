using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Common.Solid.Movable;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Elements.Templates.Liquid;

namespace PixelDust.Game.Elements.Common.Liquid
{
    [PElementRegister(9)]
    public class PLava : PLiquid
    {
        protected override void OnSettings()
        {
            this.Name = "Lava";
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_10");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_10");

            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());

            this.DefaultTemperature = 1000;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue < 500)
            {
                this.Context.ReplaceElement<PStone>();
                this.Context.SetElementTemperature(550);
            }
        }
    }
}