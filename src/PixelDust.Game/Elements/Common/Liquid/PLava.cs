using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.Items;
using PixelDust.Game.Elements.Common.Solid.Movable;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Items;

namespace PixelDust.Game.Elements.Common.Liquid
{
    [PGameContent]
    [PElementRegister(9)]
    [PItemRegister(typeof(PLavaItem))]
    public class PLava : PLiquid
    {
        private sealed class PLavaItem : PItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_LAVA";
                this.Name = "Lava";
                this.Description = string.Empty;
                this.Category = string.Empty;
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_10");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(PLava);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.Game.AssetDatabase.GetTexture("element_10");
            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());
            this.DefaultTemperature = 1000;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue < 500)
            {
                this.Context.ReplaceElement<PStone>();
                this.Context.SetElementTemperature(550);
            }
        }
    }
}