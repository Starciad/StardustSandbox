using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Common.Gases;
using PixelDust.Game.Elements.Common.Solid.Movable;
using PixelDust.Game.Elements.Templates.Liquid;
using PixelDust.Game.Mathematics;
using PixelDust.Game.Utilities;
using PixelDust.Game.Worlding.World.Slots;

using System;

namespace PixelDust.Game.Elements.Common.Liquid
{
    [PElementRegister(3)]
    public class Water : PLiquid
    {
        protected override void OnSettings()
        {
            this.Name = "Water";
            this.Description = string.Empty;

            this.Render.AddFrame(new(2, 0));

            this.DefaultDispersionRate = 3;
            this.DefaultTemperature = 25;

            this.EnableNeighborsAction = true;
        }

        protected override void OnNeighbors(ReadOnlySpan<(Vector2Int, PWorldElementSlot)> neighbors, int length)
        {
            foreach ((Vector2Int, PWorldElementSlot) neighbor in neighbors)
            {
                if (neighbor.Item2.Instance is Dirt)
                {
                    this.Context.DestroyElement();
                    this.Context.ReplaceElement<Mud>(neighbor.Item1);
                    return;
                }

                if (neighbor.Item2.Instance is Stone)
                {
                    if (PRandom.Range(0, 150) == 0)
                    {
                        this.Context.DestroyElement();
                        this.Context.ReplaceElement<Sand>(neighbor.Item1);
                        return;
                    }
                }
            }
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 100)
            {
                this.Context.ReplaceElement<Steam>();
            }

            if (currentValue <= 0)
            {
                this.Context.ReplaceElement<Ice>();
            }
        }
    }
}