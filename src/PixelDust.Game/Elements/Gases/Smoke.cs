using PixelDust.Core.Elements;

namespace PixelDust.Game.Elements.Gases
{
    [PElementRegister]
    internal class Smoke : PGas
    {
        protected override void OnSettings()
        {
            Name = "Smoke";
            Description = string.Empty;
            Color = new(60, 61, 62);
        }
    }
}
