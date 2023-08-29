using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Solid.Immovable
{
    [PElementRegister(11)]
    internal sealed class Glass : PImmovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Glass";
            Description = string.Empty;

            Render = new();
            Render.AddFrame(new(1, 1));
        }
    }
}