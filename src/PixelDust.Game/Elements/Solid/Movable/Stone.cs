using PixelDust.Core.Elements.Attributes;
using PixelDust.Core.Elements.Types.Solid;
using PixelDust.Game.Elements.Liquid;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister(4)]
    internal sealed class Stone : PMovableSolid
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
                _ = this.Context.TryReplace<Lava>(this.Context.Position);
                _ = this.Context.TrySetTemperature(this.Context.Position, 600);
            }
        }
    }
}
