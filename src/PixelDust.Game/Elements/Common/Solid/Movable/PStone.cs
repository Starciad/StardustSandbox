using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Common.Liquid;
using PixelDust.Game.Elements.Templates.Solid;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PElementRegister(3)]
    public sealed class PStone : PMovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Stone";
            this.Description = string.Empty;

            this.Render.AddFrame(new(3, 0));

            this.DefaultTemperature = 20;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 500)
            {
                this.Context.ReplaceElement<PLava>();
                this.Context.SetElementTemperature(600);
            }
        }
    }
}
