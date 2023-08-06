using PixelDust.Core;

namespace PixelDust
{
    [PElementRegister]
    internal sealed class Salt : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Salt";
            Description = string.Empty;
            Color = new(168, 172, 172);
        }
    }
}
