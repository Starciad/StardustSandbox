using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.Items;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Items;

namespace PixelDust.Game.Elements.Common.Solid.Immovable
{
    [PGameContent]
    [PElementRegister(14)]
    [PItemRegister(typeof(PWoodItem))]
    public sealed class PWood : PImmovableSolid
    {
        private sealed class PWoodItem : PItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_WOOD";
                this.Name = "Wood";
                this.Description = string.Empty;
                this.Category = "Solids";
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_15");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(PWood);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.Game.AssetDatabase.GetTexture("element_15");
            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());
            this.DefaultTemperature = 20;
        }
    }
}
