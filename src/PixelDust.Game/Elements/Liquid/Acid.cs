using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;
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

            Render = new();
            Render.AddFrame(new(0, 1));

            EnableNeighborsAction = true;
        }

        protected override void OnNeighbors((Vector2, PWorldSlot)[] neighbors, int length)
        {
            foreach ((Vector2, PWorldSlot) neighbor in neighbors)
            {
                if (neighbor.Item2.Element is Acid ||
                    neighbor.Item2.Element is Wall)
                    continue;

                PElementContext.TryDestroy(PElementContext.Position);
                PElementContext.TryDestroy(neighbor.Item1);
            }
        }
    }
}