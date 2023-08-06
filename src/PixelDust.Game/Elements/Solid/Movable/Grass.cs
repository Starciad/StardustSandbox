using Microsoft.Xna.Framework;

using PixelDust.Core.Elements;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister]
    internal sealed class Grass : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Grass";
            Description = string.Empty;
            Color = new Color(70, 115, 2);
        }
    }
}
