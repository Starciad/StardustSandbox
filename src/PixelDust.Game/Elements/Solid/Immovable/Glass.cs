using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Solid.Immovable
{
    [PElementRegister]
    internal sealed class Glass : PImmovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Glass";
            Description = string.Empty;
            Color = new(252, 249, 243);

            TileSet = new(PContent.Load<Texture2D>("Sprites/Tiles/Tile_11"));
        }
    }
}