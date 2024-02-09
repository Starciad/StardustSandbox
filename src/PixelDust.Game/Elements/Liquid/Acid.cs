using PixelDust.Core.Elements.Attributes;
using PixelDust.Core.Elements.Templates.Liquid;
using PixelDust.Core.Worlding.World.Slots;
using PixelDust.Game.Elements.Solid.Immovable;
using PixelDust.Mathematics;

using System;

namespace PixelDust.Game.Elements.Liquid
{
    [PElementRegister(11)]
    internal class Acid : PLiquid
    {
        protected override void OnSettings()
        {
            this.Name = "Acid";
            this.Description = string.Empty;

            this.Render.AddFrame(new(0, 1));

            this.DefaultTemperature = 10;

            this.EnableNeighborsAction = true;
        }

        protected override void OnNeighbors(ReadOnlySpan<(Vector2Int, PWorldElementSlot)> neighbors, int length)
        {
            foreach ((Vector2Int, PWorldElementSlot) neighbor in neighbors)
            {
                if (neighbor.Item2.Instance is Acid ||
                    neighbor.Item2.Instance is Wall)
                {
                    continue;
                }

                this.Context.DestroyElement();
                this.Context.DestroyElement(neighbor.Item1);
            }
        }
    }
}