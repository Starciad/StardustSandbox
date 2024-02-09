using PixelDust.Core.Elements.Attributes;
using PixelDust.Core.Elements.Templates.Gases;

namespace PixelDust.Game.Elements.Gases
{
    [PElementRegister(19)]
    internal class Steam : PGas
    {
        protected override void OnSettings()
        {
            this.Name = "Steam";
            this.Description = string.Empty;

            this.Render.AddFrame(new(8, 1));

            this.DefaultTemperature = 100;
        }
    }
}
