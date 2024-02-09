using PixelDust.Core.Elements.Attributes;
using PixelDust.Core.Elements.Templates.Solid;

namespace PixelDust.Game.Elements.Solid.Immovable
{
    [PElementRegister(13)]
    internal sealed class Metal : PImmovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Metal";
            this.Description = string.Empty;

            this.Render.AddFrame(new(2, 1));

            this.DefaultTemperature = 30;
        }
    }
}
