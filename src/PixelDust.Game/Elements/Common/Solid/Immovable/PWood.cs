using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Templates.Solid;

namespace PixelDust.Game.Elements.Common.Solid.Immovable
{
    [PElementRegister(14)]
    public sealed class PWood : PImmovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Wood";
            this.Description = string.Empty;

            this.Render.AddFrame(new(4, 1));

            this.DefaultTemperature = 20;
        }
    }
}
