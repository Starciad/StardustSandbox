using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Common.Solid.Movable;
using PixelDust.Game.Elements.Templates.Liquid;

namespace PixelDust.Game.Elements.Common.Liquid
{
    [PElementRegister(10)]
    public class Lava : PLiquid
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
                this.Context.ReplaceElement<Stone>();
                this.Context.SetElementTemperature(550);
            }
        }
    }
}