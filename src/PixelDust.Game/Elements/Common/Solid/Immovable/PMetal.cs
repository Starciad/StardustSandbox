using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.Items;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Items;

namespace PixelDust.Game.Elements.Common.Solid.Immovable
{
    [PGameContent]
    [PElementRegister(12)]
    [PItemRegister(typeof(PMetalItem))]
    public sealed class PMetal : PImmovableSolid
    {
        private sealed class PMetalItem : PItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_METAL";
                this.Name = "Metal";
                this.Description = string.Empty;
                this.Category = string.Empty;
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_13");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(PMetal);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.Game.AssetDatabase.GetTexture("element_13");
            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());
            this.DefaultTemperature = 30;
        }
    }
}
