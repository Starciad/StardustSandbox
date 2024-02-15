using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Templates.Gases;

namespace PixelDust.Game.Elements.Common.Gases
{
    [PElementRegister(19)]
    public class PSmoke : PGas
    {
        protected override void OnSettings()
        {
            this.Name = "Smoke";
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_1");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_20");

            this.DefaultTemperature = 100;
        }
    }
}
