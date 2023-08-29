using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Gases
{
    [PElementRegister]
    internal class Smoke : PGas
    {
        protected override void OnSettings()
        {
            Name = "Smoke";
            Description = string.Empty;
            Color = new(60, 61, 62);

            TileSet = new(PContent.Load<Texture2D>("Sprites/Tiles/Tile_18"));
        }
    }
}
