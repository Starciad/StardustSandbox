using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.Items;
using PixelDust.Game.Elements.Common.Solid.Movable;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Items;

namespace PixelDust.Game.Elements.Common.Solid.Immovable
{
    [PGameContent]
    [PElementRegister(11)]
    [PItemRegister(typeof(PGlassItem))]
    public sealed class PGlass : PImmovableSolid
    {
        private sealed class PGlassItem : PItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_GLASS";
                this.Name = "Glass";
                this.Description = string.Empty;
                this.Category = string.Empty;
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_12");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(PGlass);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.Game.AssetDatabase.GetTexture("element_12");
            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());
            this.DefaultTemperature = 25;
        }
    }
}