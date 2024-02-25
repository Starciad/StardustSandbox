using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.Items;
using PixelDust.Game.Elements.Common.Liquid;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Items;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PGameContent]
    [PElementRegister(3)]
    [PItemRegister(typeof(PStoneItem))]
    public sealed class PStone : PMovableSolid
    {
        private sealed class PStoneItem : PItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_STONE";
                this.Name = "Stone";
                this.Description = string.Empty;
                this.Category = string.Empty;
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_4");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(PStone);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.Game.AssetDatabase.GetTexture("element_4");
            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());
            this.DefaultTemperature = 20;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 500)
            {
                this.Context.ReplaceElement<PLava>();
                this.Context.SetElementTemperature(600);
            }
        }
    }
}
