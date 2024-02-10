using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Templates.Solid;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PElementRegister(2)]
    public sealed class Mud : PMovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Mud";
            this.Description = string.Empty;

            this.Render.AddFrame(new(1, 0));

            this.DefaultTemperature = 18;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 100)
            {
                this.Context.ReplaceElement<Dirt>();
            }
        }
    }
}
