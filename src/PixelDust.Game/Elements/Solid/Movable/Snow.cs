using PixelDust.Core.Elements.Attributes;
using PixelDust.Core.Elements.Types.Solid;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister(8)]
    internal sealed class Snow : PMovableSolid
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
