using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Elements.Rendering.Common;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PElementRegister(1)]
    public sealed class PMud : PMovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Mud";
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_2");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_2");

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
