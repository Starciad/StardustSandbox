using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.Items;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Items;

namespace PixelDust.Game.Elements.Common.Solid.Immovable
{
    [PGameContent]
    [PElementRegister(13)]
    [PItemRegister(typeof(PWallItem))]
    public sealed class PWall : PImmovableSolid
    {
        private sealed class PWallItem : PItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_WALL";
                this.Name = "Wall";
                this.Description = string.Empty;
                this.Category = "Solids";
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_14");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(PWall);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.Game.AssetDatabase.GetTexture("element_14");
            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());
            this.EnableTemperature = false;
        }
    }
}
