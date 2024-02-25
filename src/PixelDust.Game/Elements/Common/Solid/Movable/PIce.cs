using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.Items;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Items;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PGameContent]
    [PElementRegister(5)]
    [PItemRegister(typeof(PIceItem))]
    public sealed class PIce : PMovableSolid
    {
        private sealed class PIceItem : PItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_ICE";
                this.Name = "Ice";
                this.Description = string.Empty;
                this.Category = string.Empty;
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_6");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(PIce);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.Game.AssetDatabase.GetTexture("element_6");
            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());
            this.DefaultTemperature = 0;
        }
    }
}
