using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Solid.Immovable
{
    [PElementRegister]
    internal sealed class Metal : PImmovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Metal";
            Description = string.Empty;
            Color = new(80, 80, 80);

            TileSet = new(PContent.Load<Texture2D>("Sprites/Tiles/Tile_12"));
        }
    }
}
