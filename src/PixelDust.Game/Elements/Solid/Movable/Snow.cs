using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister]
    internal sealed class Snow : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Snow";
            Description = string.Empty;
            Color = new(185, 232, 232);

            TileSet = new(PContent.Load<Texture2D>("Sprites/Tiles/Tile_7"));
        }
    }
}
