using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Templates.Gases;

namespace PixelDust.Game.Elements.Common.Gases
{
    [PElementRegister(19)]
    public class PSmoke : PGas
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
