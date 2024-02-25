using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.Items;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Items;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PGameContent]
    [PElementRegister(0)]
    [PItemRegister(typeof(PDirtItem))]
    public sealed class PDirt : PMovableSolid
    {
        private sealed class PDirtItem : PItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "DIRT";
                this.Name = "Dirt";
                this.Description = string.Empty;
                this.Category = string.Empty;
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_1");
                this.IsHidden = false;
                this.LevelRequired = 0;
                this.ReferencedType = typeof(PDirt);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.Game.AssetDatabase.GetTexture("element_1");
            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());
            this.DefaultTemperature = 20;
        }
    }
}
