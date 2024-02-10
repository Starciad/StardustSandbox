using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Templates.Solid;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PElementRegister(7)]
    public sealed class PSnow : PMovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Snow";
            this.Description = string.Empty;

            this.Render.AddFrame(new(7, 0));

            this.DefaultTemperature = -5;
        }
    }
}
