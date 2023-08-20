using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister]
    internal sealed class Dirt : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Dirt";
            Description = string.Empty;
            Color = new Color(79, 58, 43);

            TileSet = new(PContent.Load<Texture2D>("Sprites/Tiles/Tile_1"));
        }
    }
}
