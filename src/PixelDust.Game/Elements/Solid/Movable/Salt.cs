using PixelDust.Core.Elements;

namespace PixelDust.Game.Elements.Solid.Movable
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
