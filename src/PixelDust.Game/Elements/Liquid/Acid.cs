using PixelDust.Core.Elements;
using PixelDust.Core.Mathematics;
using PixelDust.Core.Worlding;

using PixelDust.Game.Elements.Solid.Immovable;

namespace PixelDust.Game.Elements.Liquid
{
    [PElementRegister(16)]
    internal class Acid : PLiquid
    {
        protected override void OnSettings()
        {
            Name = "Acid";
            Description = string.Empty;

            
            Render.AddFrame(new(0, 1));

            EnableNeighborsAction = true;
        }

        protected override void OnNeighbors((Vector2Int, PWorldElementSlot)[] neighbors, int length)
        {
            foreach ((Vector2Int, PWorldElementSlot) neighbor in neighbors)
            {
                if (neighbor.Item2.Instance is Acid ||
                    neighbor.Item2.Instance is Wall)
                    continue;

                Context.TryDestroy(Context.Position);
                Context.TryDestroy(neighbor.Item1);
            }
        }
    }
}