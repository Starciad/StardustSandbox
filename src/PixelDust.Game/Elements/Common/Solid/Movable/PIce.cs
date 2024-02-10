using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Templates.Solid;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PElementRegister(5)]
    public sealed class PIce : PMovableSolid
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
