using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.Items;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Items;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PGameContent]
    [PElementRegister(7)]
    [PItemRegister(typeof(PSnowItem))]
    public sealed class PSnow : PMovableSolid
    {
        private sealed class PSnowItem : PItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_SNOW";
                this.Name = "Snow";
                this.Description = string.Empty;
                this.Category = string.Empty;
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_8");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(PSnow);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.Game.AssetDatabase.GetTexture("element_8");
            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());
            this.DefaultTemperature = -5;
        }
    }
}
