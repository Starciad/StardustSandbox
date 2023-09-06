using PixelDust.Core.Elements;

namespace PixelDust.Game.Elements.Gases
{
    [PElementRegister(20)]
    internal class Smoke : PGas
    {
        protected override void OnSettings()
        {
            Name = "Smoke";
            Description = string.Empty;

            Render.AddFrame(new(9, 1));

            DefaultTemperature = 100;
        }
    }
}
