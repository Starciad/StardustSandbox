using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Solid.Immovable
{
    [PElementRegister(14)]
    internal sealed class Wood : PImmovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Wood";
            Description = string.Empty;

            
            Render.AddFrame(new(4, 1));
        }
    }
}
