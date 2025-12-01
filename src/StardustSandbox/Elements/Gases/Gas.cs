using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;
using StardustSandbox.Extensions;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Gases
{
    internal abstract class Gas : Element
    {
        private readonly List<Point> emptyPositionsCache = [];
        private readonly List<Point> validPositionsCache = [];

        protected override void OnStep(ElementContext context)
        {
            this.emptyPositionsCache.Clear();
            this.validPositionsCache.Clear();

            foreach (Point position in context.Slot.Position.GetNeighboringCardinalPoints())
            {
                if (context.IsEmptySlotLayer(position, context.Layer))
                {
                    this.emptyPositionsCache.Add(position);
                }
                else if (context.TryGetSlot(position, out Slot value))
                {
                    SlotLayer worldSlotLayer = value.GetLayer(context.Layer);

                    if (worldSlotLayer.Element.Category == ElementCategory.Gas ||
                        worldSlotLayer.Element.Category == ElementCategory.Liquid)
                    {
                        if ((worldSlotLayer.Element.GetType() == GetType() && worldSlotLayer.Temperature > context.SlotLayer.Temperature) || worldSlotLayer.Element.DefaultDensity < this.DefaultDensity)
                        {
                            this.validPositionsCache.Add(position);
                        }
                    }
                }
            }

            if (this.emptyPositionsCache.Count > 0)
            {
                context.SetPosition(this.emptyPositionsCache.GetRandomItem());
            }
            else if (this.validPositionsCache.Count > 0)
            {
                Point targetPosition = this.validPositionsCache.GetRandomItem();
                _ = context.TrySwappingElements(targetPosition);
            }
        }
    }
}
