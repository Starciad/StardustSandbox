using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Solid.Immovable
{
    [PElementRegister]
    internal sealed class Wall : PImmovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Wall";
            Description = string.Empty;
            Color = Color.Gray;

            TileSet = new(PContent.Load<Texture2D>("Sprites/Tiles/Tile_13"));
        }
    }
}
