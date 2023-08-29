using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Solid.Immovable
{
    [PElementRegister(12)]
    internal sealed class Metal : PImmovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Metal";
            Description = string.Empty;

            Render = new();
            Render.AddFrame(new(2, 1));
        }
    }
}
