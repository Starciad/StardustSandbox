using PixelDust.Core.Elements.Attributes;
using PixelDust.Core.Elements.Templates.Solid;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister(6)]
    internal sealed class Ice : PMovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Ice";
            this.Description = string.Empty;

            this.Render.AddFrame(new(5, 0));

            this.DefaultTemperature = 0;
        }
    }
}
