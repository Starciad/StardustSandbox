using Microsoft.Xna.Framework;

using PixelDust.Core.Elements;
using PixelDust.Core.Worlding;
using PixelDust.Game.Elements.Gases;
using PixelDust.Game.Elements.Solid.Immovable;
using PixelDust.Game.Elements.Solid.Movable;

using SharpDX.Direct3D9;

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
                    Context.TryReplace<Stone>(Context.Position);
                    Context.TryReplace<Steam>(neighbor.Item1);
                    return;
                }

                if (neighbor.Item2.Element is Sand)
                {
                    Context.TryReplace<Glass>(neighbor.Item1);
                    return;
                }

                if (neighbor.Item2.Element is Grass)
                {
                    Context.TryDestroy(neighbor.Item1);
                    return;
                }
            }
        }
    }
}