using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Solid.Immovable
{
    [PElementRegister]
    internal sealed class Wood : PImmovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Wood";
            Description = string.Empty;
            Color = new(43, 24, 12);

            TileSet = new(PContent.Load<Texture2D>("Sprites/Tiles/Tile_14"));
        }
    }
}
