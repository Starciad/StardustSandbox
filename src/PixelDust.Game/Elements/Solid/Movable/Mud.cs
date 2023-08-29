using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister]
    internal sealed class Mud : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Mud";
            Description = string.Empty;
            Color = new Color(51, 36, 25);

            TileSet = new(PContent.Load<Texture2D>("Sprites/Tiles/Tile_2"));
        }
    }
}
