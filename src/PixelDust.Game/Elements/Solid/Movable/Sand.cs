using PixelDust.Core.Elements.Attributes;
using PixelDust.Core.Elements.Templates.Solid;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister(7)]
    internal sealed class Sand : PMovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Sand";
            this.Description = string.Empty;

            this.Render.AddFrame(new(6, 0));

            this.DefaultTemperature = 22;
        }
    }
}
