using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister(7)]
    internal sealed class Snow : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Snow";
            Description = string.Empty;

            
            Render.AddFrame(new(7, 0));
        }
    }
}
