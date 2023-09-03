using PixelDust.Core.Elements;
using PixelDust.Game.Elements.Liquid;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister(4)]
    internal sealed class Stone : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Stone";
            Description = string.Empty;

            Render.AddFrame(new(3, 0));

            DefaultTemperature = 20;
        }

        protected override void OnTemperatureChanged(float currentValue)
        {
            if (currentValue > 600)
            {
                Context.TryReplace<Lava>(Context.Position);
                Context.TrySetTemperature(Context.Position, 700);
            }
        }
    }
}
