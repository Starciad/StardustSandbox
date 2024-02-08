using PixelDust.Core.Elements.Attributes;
using PixelDust.Core.Elements.Types.Solid;

namespace PixelDust.Game.Elements.Solid.Immovable
{
    [PElementRegister(14)]
    internal sealed class Wall : PImmovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Wall";
            this.Description = string.Empty;

            this.Render.AddFrame(new(3, 1));

            this.EnableTemperature = false;
        }
    }
}
