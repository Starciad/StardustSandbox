using PixelDust.Core.Elements;
using PixelDust.Game.Elements.Gases;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister(2)]
    internal sealed class Mud : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Mud";
            Description = string.Empty;

            Render.AddFrame(new(1, 0));

            DefaultTemperature = 18;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 100)
            {
                Context.TryReplace<Dirt>(Context.Position);
            }
        }
    }
}
