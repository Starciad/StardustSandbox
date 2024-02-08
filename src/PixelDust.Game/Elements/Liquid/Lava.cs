using PixelDust.Core.Elements.Attributes;
using PixelDust.Core.Elements.Types.Liquid;
using PixelDust.Game.Elements.Solid.Movable;

namespace PixelDust.Game.Elements.Liquid
{
    [PElementRegister(10)]
    internal class Lava : PLiquid
    {
        protected override void OnSettings()
        {
            this.Name = "Lava";
            this.Description = string.Empty;

            this.Render.AddFrame(new(9, 0));

            this.DefaultTemperature = 1000;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue < 500)
            {
                _ = this.Context.TryReplace<Stone>(this.Context.Position);
                _ = this.Context.TrySetTemperature(this.Context.Position, 550);
            }
        }
    }
}