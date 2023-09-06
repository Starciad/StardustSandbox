using PixelDust.Core.Elements;
using PixelDust.Core.Mathematics;
using PixelDust.Core.Utilities;
using PixelDust.Core.Worlding;
using PixelDust.Game.Elements.Gases;
using PixelDust.Game.Elements.Solid.Movable;

using System;

namespace PixelDust.Game.Elements.Liquid
{
    [PElementRegister(3)]
    internal class Water : PLiquid
    {
        protected override void OnSettings()
        {
            Name = "Water";
            Description = string.Empty;

            Render.AddFrame(new(2, 0));

            DefaultDispersionRate = 3;
            DefaultTemperature = 25;

            EnableNeighborsAction = true;
        }

        protected override void OnNeighbors(ReadOnlySpan<(Vector2Int, PWorldElementSlot)> neighbors, int length)
        {
            foreach ((Vector2Int, PWorldElementSlot) neighbor in neighbors)
            {
                if (neighbor.Item2.Instance is Dirt)
                {
                    Context.TryDestroy(Context.Position);
                    Context.TryReplace<Mud>(neighbor.Item1);
                    return;
                }

                if (neighbor.Item2.Instance is Stone)
                {
                    if (PRandom.Range(0, 150) == 0)
                    {
                        Context.TryDestroy(Context.Position);
                        Context.TryReplace<Sand>(neighbor.Item1);
                        return;
                    }
                }
            }
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 100)
            {
                Context.TryReplace<Steam>(Context.Position);
            }

            if (currentValue <= 0)
            {
                Context.TryReplace<Ice>(Context.Position);
            }
        }
    }
}