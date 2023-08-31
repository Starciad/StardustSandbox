using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Solid.Immovable
{
    [PElementRegister(13)]
    internal sealed class Wall : PImmovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Wall";
            Description = string.Empty;

            
            Render.AddFrame(new(3, 1));
        }
    }
}
