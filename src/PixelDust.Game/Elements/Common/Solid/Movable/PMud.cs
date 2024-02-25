using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.Items;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Items;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PGameContent]
    [PElementRegister(1)]
    [PItemRegister(typeof(PMudItem))]
    public sealed class PMud : PMovableSolid
    {
        private sealed class PMudItem : PItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_MUD";
                this.Name = "Mud";
                this.Description = string.Empty;
                this.Category = "Powders";
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_2");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(PMud);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.Game.AssetDatabase.GetTexture("element_2");
            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());
            this.DefaultTemperature = 18;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 100)
            {
                this.Context.ReplaceElement<PDirt>();
            }
        }
    }
}
