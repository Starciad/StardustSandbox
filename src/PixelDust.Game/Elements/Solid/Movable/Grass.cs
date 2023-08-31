using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister(6)]
    internal sealed class Grass : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Grass";
            Description = string.Empty;

            
            Render.AddFrame(new(4, 0));
        }
    }
}
