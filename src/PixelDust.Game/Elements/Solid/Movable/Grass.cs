using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister]
    internal sealed class Grass : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Grass";
            Description = string.Empty;
            Color = new Color(70, 115, 2);

            TileSet = new(PContent.Load<Texture2D>("Sprites/Tiles/Tile_6"));
        }
    }
}
