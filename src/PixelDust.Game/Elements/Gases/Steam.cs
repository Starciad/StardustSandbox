using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Gases
{
    [PElementRegister]
    internal class Steam : PGas
    {
        protected override void OnSettings()
        {
            Name = "Steam";
            Description = string.Empty;
            Color = new(196, 228, 243);

            TileSet = new(PContent.Load<Texture2D>("Sprites/Tiles/Tile_19"));
        }
    }
}
