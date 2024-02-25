using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Elements.Rendering.Common;

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
            this.Texture = this.Game.AssetDatabase.GetTexture("element_19");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_19");

            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());

            this.DefaultTemperature = 100;
        }
    }
}
