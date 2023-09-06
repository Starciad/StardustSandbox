using PixelDust.Core.Elements;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister(5)]
    internal sealed class Grass : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Grass";
            Description = string.Empty;

            Render.AddFrame(new(4, 0));

            DefaultTemperature = 22;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 200)
            {
                Context.TryDestroy(Context.Position);
            }
        }
    }
}
