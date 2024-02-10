using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Templates.Solid;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PElementRegister(0)]
    public sealed class PDirt : PMovableSolid
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
