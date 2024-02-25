using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Elements.Rendering.Common;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PElementRegister(4)]
    public sealed class PGrass : PMovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Grass";
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_5");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_5");

            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());

            this.DefaultTemperature = 22;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 200)
            {
                this.Context.DestroyElement();
            }
        }
    }
}
