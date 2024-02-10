using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Templates.Solid;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PElementRegister(1)]
    public sealed class Dirt : PMovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Dirt";
            this.Description = string.Empty;

            this.Render.AddFrame(new(0, 0));

            this.DefaultTemperature = 20;
        }
    }
}
