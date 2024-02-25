using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.Items;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Items;

namespace PixelDust.Game.Elements.Common.Gases
{
    [PGameContent]
    [PElementRegister(19)]
    [PItemRegister(typeof(PSmokeItem))]
    public class PSmoke : PGas
    {
        private sealed class PSmokeItem : PItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_SMOKE";
                this.Name = "Smoke";
                this.Description = string.Empty;
                this.Category = "Gases";
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_20");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(PSmoke);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.Game.AssetDatabase.GetTexture("element_20");
            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());
            this.DefaultTemperature = 100;
        }
    }
}
