using PixelDust.Core.Elements;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister]
    internal sealed class Snow : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Snow";
            Description = string.Empty;
            Color = new(185, 232, 232);
        }
    }
}
