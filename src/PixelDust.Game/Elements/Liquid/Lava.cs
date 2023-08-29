using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;
using PixelDust.Core.Worlding;

using PixelDust.Game.Elements.Gases;
using PixelDust.Game.Elements.Solid.Movable;

namespace PixelDust.Game.Elements.Liquid
{
    [PElementRegister(15)]
    internal class Lava : PLiquid
    {
        protected override void OnSettings()
        {
            Name = "Lava";
            Description = string.Empty;

            Render = new();
            Render.AddFrame(new(9, 0));

            EnableNeighborsAction = true;
        }

        protected override void OnNeighbors((Vector2, PWorldSlot)[] neighbors, int length)
        {
            foreach ((Vector2, PWorldSlot) neighbor in neighbors)
            {
                if (neighbor.Item2.Element is Stone)
                {
                    PElementContext.TryReplace<Lava>(neighbor.Item1);
                    return;
                }

                if (neighbor.Item2.Element is Water)
                {
                    PElementContext.TryDestroy(PElementContext.Position);
                    PElementContext.TryReplace<Steam>(neighbor.Item1);
                    return;
                }

                if (neighbor.Item2.Element is Grass)
                {
                    PElementContext.TryDestroy(neighbor.Item1);
                    return;
                }


                if (neighbor.Item2.Element is Mud)
                {
                    PElementContext.TryReplace<Dirt>(neighbor.Item1);
                    return;
                }
            }
        }
    }
}