using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Templates.Gases;

namespace PixelDust.Game.Elements.Common.Gases
{
    [PElementRegister(18)]
    public class PSteam : PGas
    {
        protected override void OnSettings()
        {
            this.Name = "Steam";
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_1");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_19");

            this.DefaultTemperature = 100;
        }
    }
}
