using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister]
    internal sealed class Sand : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Sand";
            Description = string.Empty;
            Color = new(203, 165, 95);

            TileSet = new(PContent.Load<Texture2D>("Sprites/Tiles/Tile_5"));
        }
    }
}
