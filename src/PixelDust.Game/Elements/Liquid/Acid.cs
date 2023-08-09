using Microsoft.Xna.Framework;

using PixelDust.Core.Elements;
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