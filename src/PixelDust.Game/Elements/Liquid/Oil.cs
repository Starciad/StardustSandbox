using PixelDust.Core.Elements.Attributes;
using PixelDust.Core.Elements.Templates.Liquid;

namespace PixelDust.Game.Elements.Liquid
{
    [PElementRegister(21)]
    internal class Oil : PLiquid
    {
        protected override void OnSettings()
        {
            this.Name = "Oil";
            this.Description = string.Empty;

            this.DefaultTemperature = 30;
        }
    }
}
