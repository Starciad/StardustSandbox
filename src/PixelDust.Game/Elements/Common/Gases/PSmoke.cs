using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Elements.Rendering.Common;

namespace PixelDust.Game.Elements.Common.Gases
{
    [PGameContent]
    [PElementRegister(19)]
    public class PSmoke : PGas
    {
        protected override void OnSettings()
        {
            this.Name = "Smoke";
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_20");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_20");

            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());

            this.DefaultTemperature = 100;
        }
    }
}
