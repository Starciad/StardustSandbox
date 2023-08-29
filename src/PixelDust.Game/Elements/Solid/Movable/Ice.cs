using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister(8)]
    internal sealed class Ice : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Ice";
            Description = string.Empty;

            Render = new();
            Render.AddFrame(new(5, 0));
        }
    }
}
