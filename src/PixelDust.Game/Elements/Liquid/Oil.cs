using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Liquid
{
    [PElementRegister]
    internal class Oil : PLiquid
    {
        protected override void OnSettings()
        {
            Name = "Oil";
            Description = string.Empty;
            Color = new(236, 162, 55);

            TileSet = new(PContent.Load<Texture2D>("Sprites/Tiles/Tile_17"));
        }
    }
}
