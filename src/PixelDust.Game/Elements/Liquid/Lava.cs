using PixelDust.Core.Elements;
using PixelDust.Core.Mathematics;
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

            
            Render.AddFrame(new(9, 0));

            EnableNeighborsAction = true;
        }

        protected override void OnNeighbors((Vector2Int, PWorldElementSlot)[] neighbors, int length)
        {
            foreach ((Vector2Int, PWorldElementSlot) neighbor in neighbors)
            {
                if (neighbor.Item2.Instance is Stone)
                {
                    Context.TryReplace<Lava>(neighbor.Item1);
                    return;
                }

                if (neighbor.Item2.Instance is Water)
                {
                    Context.TryDestroy(Context.Position);
                    Context.TryReplace<Steam>(neighbor.Item1);
                    return;
                }

                if (neighbor.Item2.Instance is Grass)
                {
                    Context.TryDestroy(neighbor.Item1);
                    return;
                }


                if (neighbor.Item2.Instance is Mud)
                {
                    Context.TryReplace<Dirt>(neighbor.Item1);
                    return;
                }
            }
        }
    }
}