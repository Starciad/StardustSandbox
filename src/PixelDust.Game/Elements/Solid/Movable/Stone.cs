using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister]
    internal sealed class Stone : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Stone";
            Description = string.Empty;
            Color = new(90, 90, 90);

            TileSet = new(PContent.Load<Texture2D>("Sprites/Tiles/Tile_4"));
        }
    }
}
