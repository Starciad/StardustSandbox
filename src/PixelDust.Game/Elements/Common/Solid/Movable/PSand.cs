using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.Items;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Items;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PGameContent]
    [PElementRegister(6)]
    [PItemRegister(typeof(PSandItem))]
    public sealed class PSand : PMovableSolid
    {
        private sealed class PSandItem : PItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_SAND";
                this.Name = "Sand";
                this.Description = string.Empty;
                this.Category = "Powders";
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_7");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(PSand);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.Game.AssetDatabase.GetTexture("element_7");
            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());
            this.DefaultTemperature = 22;
        }
    }
}
