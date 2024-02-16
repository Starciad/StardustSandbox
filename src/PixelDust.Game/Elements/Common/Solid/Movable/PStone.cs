using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Common.Liquid;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Elements.Templates.Solid;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PElementRegister(3)]
    public sealed class PStone : PMovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Stone";
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_4");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_4");

            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());

            this.DefaultTemperature = 20;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 500)
            {
                this.Context.ReplaceElement<PLava>();
                this.Context.SetElementTemperature(600);
            }
        }
    }
}
