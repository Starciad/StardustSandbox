using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister]
    internal sealed class Ice : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Ice";
            Description = string.Empty;
            Color = new Color(63, 208, 212);

            TileSet = new(PContent.Load<Texture2D>("Sprites/Tiles/Tile_8"));
        }
    }
}
