using PixelDust.Core.Elements.Attributes;
using PixelDust.Core.Elements.Templates.Solid;

namespace PixelDust.Game.Elements.Solid.Immovable
{
    [PElementRegister(15)]
    internal sealed class Wood : PImmovableSolid
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
