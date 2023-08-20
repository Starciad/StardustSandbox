using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;
using PixelDust.Core.Worlding;

using PixelDust.Game.Elements.Gases;
using PixelDust.Game.Elements.Solid.Movable;

namespace PixelDust.Game.Elements.Liquid
{
    [PElementRegister]
    internal class Lava : PLiquid
    {
        protected override void OnSettings()
        {
            Name = "Lava";
            Description = string.Empty;
            Color = new(255, 116, 0);

            TileSet = new(PContent.Load<Texture2D>("Sprites/Tiles/Tile_15"));
        }

        protected override void OnNeighbors((Vector2, PWorldSlot)[] neighbors, int length)
        {
            foreach ((Vector2, PWorldSlot) neighbor in neighbors)
            {
                if (neighbor.Item2.Element is Stone)
                {
                    Context.TryReplace<Lava>(neighbor.Item1);
                    return;
                }

                if (neighbor.Item2.Element is Water)
                {
                    Context.TryDestroy(Context.Position);
                    Context.TryReplace<Steam>(neighbor.Item1);
                    return;
                }

                if (neighbor.Item2.Element is Grass)
                {
                    Context.TryDestroy(neighbor.Item1);
                    return;
                }


                if (neighbor.Item2.Element is Mud)
                {
                    Context.TryReplace<Dirt>(neighbor.Item1);
                    return;
                }
            }
        }
    }
}