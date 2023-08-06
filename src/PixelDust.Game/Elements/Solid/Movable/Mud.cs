using Microsoft.Xna.Framework;

using PixelDust.Core.Elements;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister]
    internal sealed class Mud : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Mud";
            Description = string.Empty;
            Color = new Color(51, 36, 25);
        }
    }
}
