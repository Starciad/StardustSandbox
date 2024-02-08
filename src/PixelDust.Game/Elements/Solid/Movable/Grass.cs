using PixelDust.Core.Elements.Attributes;
using PixelDust.Core.Elements.Types.Solid;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister(5)]
    internal sealed class Grass : PMovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Grass";
            this.Description = string.Empty;

            this.Render.AddFrame(new(4, 0));

            this.DefaultTemperature = 22;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 200)
            {
                _ = this.Context.TryDestroy(this.Context.Position);
            }
        }
    }
}
