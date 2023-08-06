using PixelDust.Core.Elements;

namespace PixelDust.Game.Elements.Liquid
{
    [PElementRegister]
    internal class SaltWater : PLiquid
    {
        protected override void OnSettings()
        {
            Name = "SaltWater";
            Description = string.Empty;
            Color = new(28, 163, 236);
        }
    }
}
