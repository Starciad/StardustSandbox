using PixelDust.Core.Elements.Attributes;
using PixelDust.Core.Elements.Types.Solid;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister(1)]
    internal sealed class Dirt : PMovableSolid
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
