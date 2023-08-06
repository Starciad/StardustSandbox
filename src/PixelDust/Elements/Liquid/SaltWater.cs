using PixelDust.Core;

namespace PixelDust
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
