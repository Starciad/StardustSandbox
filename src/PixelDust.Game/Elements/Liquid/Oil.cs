using PixelDust.Core.Elements;

namespace PixelDust.Game.Elements.Liquid
{
    [PElementRegister(21)]
    internal class Oil : PLiquid
    {
        protected override void OnSettings()
        {
            Name = "Oil";
            Description = string.Empty;

            DefaultTemperature = 30;
        }
    }
}
