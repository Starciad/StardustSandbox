using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Templates.Gases;

namespace PixelDust.Game.Elements.Common.Gases
{
    [PElementRegister(20)]
    public class Smoke : PGas
    {
        protected override void OnSettings()
        {
            this.Name = "Smoke";
            this.Description = string.Empty;

            this.Render.AddFrame(new(9, 1));

            this.DefaultTemperature = 100;
        }
    }
}
