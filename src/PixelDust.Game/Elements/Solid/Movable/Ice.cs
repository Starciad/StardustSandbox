using Microsoft.Xna.Framework;

using PixelDust.Core.Elements;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister]
    internal sealed class Ice : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Ice";
            Description = string.Empty;
            Color = new Color(63, 208, 212);
        }
    }
}
