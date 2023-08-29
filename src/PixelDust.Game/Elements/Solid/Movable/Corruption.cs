using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;
using PixelDust.Core.Utilities;
using PixelDust.Core.Worlding;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister]
    internal sealed class Corruption : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Corruption";
            Description = string.Empty;
            Color = new Color(34, 26, 44);

            TileSet = new(PContent.Load<Texture2D>("Sprites/Tiles/Tile_9"));
        }

        protected override void OnNeighbors((Vector2, PWorldSlot)[] neighbors, int length)
        {
            if (length == 1)
            {
                if (neighbors[0].Item2.Element is Corruption)
                    return;

                Context.TryReplace<Corruption>(neighbors[0].Item1);
            }
            else
            {
                int index = PRandom.Range(0, length);

                if (neighbors[index].Item2.Element is Corruption)
                    return;

                Context.TryReplace<Corruption>(neighbors[index].Item1);
            }
        }
    }
}
