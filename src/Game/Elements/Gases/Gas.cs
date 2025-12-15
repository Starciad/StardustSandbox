using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;
using StardustSandbox.Extensions;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Gases
{
    internal abstract class Gas : Element
    {
        private static readonly List<Point> availablePositions = [];

        private void EvaluateNeighboringPosition(ElementContext context, Point position)
        {
            if (context.IsEmptySlotLayer(position, context.Layer))
            {
                availablePositions.Add(position);
            }
            else if (context.TryGetSlot(position, out Slot value))
            {
                SlotLayer slotLayer = value.GetLayer(context.Layer);

                if (slotLayer.Element.Category == ElementCategory.Gas ||
                    slotLayer.Element.Category == ElementCategory.Liquid)
                {
                    if ((slotLayer.Element.Index == this.Index && slotLayer.Temperature > context.SlotLayer.Temperature) || this.DefaultDensity > slotLayer.Element.DefaultDensity)
                    {
                        availablePositions.Add(position);
                    }
                }
            }
        }

        protected override void OnStep(ElementContext context)
        {
            availablePositions.Clear();

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

            if (availablePositions.Count == 0)
            {
                return;
            }

            Point targetPosition = availablePositions.GetRandomItem();

            if (context.IsEmptySlotLayer(targetPosition))
            {
                context.SetPosition(targetPosition);
            }
            else
            {
                context.SwappingElements(targetPosition);
            }
        }
    }
}
