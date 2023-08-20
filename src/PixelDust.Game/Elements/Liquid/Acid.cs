using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;
using PixelDust.Core.Worlding;

using PixelDust.Game.Elements.Solid.Immovable;

namespace PixelDust.Game.Elements.Liquid
{
    [PElementRegister]
    internal class Acid : PLiquid
    {
        protected override void OnSettings()
        {
            Name = "Acid";
            Description = string.Empty;
            Color = new(0, 255, 0);

            TileSet = new(PContent.Load<Texture2D>("Sprites/Tiles/Tile_16"));
        }

        protected override void OnNeighbors((Vector2, PWorldSlot)[] neighbors, int length)
        {
            foreach ((Vector2, PWorldSlot) neighbor in neighbors)
            {
                if (neighbor.Item2.Element is Acid ||
                    neighbor.Item2.Element is Wall)
                    continue;

                Context.TryDestroy(Context.Position);
                Context.TryDestroy(neighbor.Item1);
            }
        }
    }
}