using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister(2)]
    internal sealed class Mud : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Mud";
            Description = string.Empty;

            Render = new();
            Render.AddFrame(new(1, 0));
        }
    }
}
