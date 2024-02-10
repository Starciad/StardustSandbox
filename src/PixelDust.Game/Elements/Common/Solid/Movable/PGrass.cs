using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Templates.Solid;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PElementRegister(4)]
    public sealed class PGrass : PMovableSolid
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
                this.Context.DestroyElement();
            }
        }
    }
}
