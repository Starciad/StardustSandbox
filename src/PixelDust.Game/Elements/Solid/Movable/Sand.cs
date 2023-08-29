using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister(5)]
    internal sealed class Sand : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Sand";
            Description = string.Empty;

            Render = new();
            Render.AddFrame(new(6, 0));
        }
    }
}
