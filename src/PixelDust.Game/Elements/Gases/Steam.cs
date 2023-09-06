using PixelDust.Core.Elements;

namespace PixelDust.Game.Elements.Gases
{
    [PElementRegister(19)]
    internal class Steam : PGas
    {
        protected override void OnSettings()
        {
            Name = "Steam";
            Description = string.Empty;
            
            Render.AddFrame(new(8, 1));

            DefaultTemperature = 100;
        }
    }
}
