using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Templates.Solid;

namespace PixelDust.Game.Elements.Common.Solid.Immovable
{
    [PElementRegister(11)]
    public sealed class PGlass : PImmovableSolid
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