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

        private void EvaluateNeighboringPosition(in ElementContext context, Point position)
        {
            if (context.IsEmptySlotLayer(position, context.Layer))
            {
                this.emptyPositionsCache.Add(position);
            }
            else if (context.TryGetSlot(position, out Slot value))
            {
                SlotLayer slotLayer = value.GetLayer(context.Layer);

                if (slotLayer.Element.Category == ElementCategory.Gas ||
                    slotLayer.Element.Category == ElementCategory.Liquid)
                {
                    if ((slotLayer.Element.Index == this.Index && slotLayer.Temperature > context.SlotLayer.Temperature) || slotLayer.Element.DefaultDensity < this.DefaultDensity)
                    {
                        this.validPositionsCache.Add(position);
                    }
                }
            }
        }

        protected override void OnStep(in ElementContext context)
        {
            this.emptyPositionsCache.Clear();
            this.validPositionsCache.Clear();

            int centerX = context.Slot.Position.X;
            int centerY = context.Slot.Position.Y;

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                    {
                        continue;
                    }

                    EvaluateNeighboringPosition(context, new(centerX + dx, centerY + dy));
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
