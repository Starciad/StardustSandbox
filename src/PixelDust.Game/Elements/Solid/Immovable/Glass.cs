using PixelDust.Core.Elements.Attributes;
using PixelDust.Core.Elements.Types.Solid;

namespace PixelDust.Game.Elements.Solid.Immovable
{
    [PElementRegister(12)]
    internal sealed class Glass : PImmovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Glass";
            this.Description = string.Empty;

            this.Render.AddFrame(new(1, 1));

            this.DefaultTemperature = 25;
        }
    }
}