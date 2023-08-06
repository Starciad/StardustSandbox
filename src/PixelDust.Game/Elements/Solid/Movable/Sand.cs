using PixelDust.Core.Elements;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister]
    internal sealed class Sand : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Sand";
            Description = string.Empty;
            Color = new(203, 165, 95);
        }
    }
}
