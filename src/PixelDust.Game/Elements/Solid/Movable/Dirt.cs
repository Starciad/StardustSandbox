using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister(1)]
    internal sealed class Dirt : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Dirt";
            Description = string.Empty;

            
            Render.AddFrame(new(0, 0));
        }
    }
}
