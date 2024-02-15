using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Elements.Templates.Solid;

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
            this.Texture = this.Game.AssetDatabase.GetTexture("element_1");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_5");

            this.Rendering.SetRenderingMechanism(new PElementSingleRenderingMechanism());

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
