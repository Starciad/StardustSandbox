using PixelDust.Core.Elements.Attributes;
using PixelDust.Core.Elements.Types.Gases;

namespace PixelDust.Game.Elements.Gases
{
    [PElementRegister(20)]
    internal class Smoke : PGas
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
