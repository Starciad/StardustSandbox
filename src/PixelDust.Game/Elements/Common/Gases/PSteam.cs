using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.Items;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Items;

namespace PixelDust.Game.Elements.Common.Gases
{
    [PGameContent]
    [PElementRegister(18)]
    [PItemRegister(typeof(PSteamItem))]
    public class PSteam : PGas
    {
        private sealed class PSteamItem : PItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_STEAM";
                this.Name = "Steam";
                this.Description = string.Empty;
                this.Category = string.Empty;
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_19");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(PSteam);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.Game.AssetDatabase.GetTexture("element_19");
            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());
            this.DefaultTemperature = 100;
        }
    }
}
