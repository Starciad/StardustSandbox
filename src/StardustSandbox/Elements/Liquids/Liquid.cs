using Microsoft.Xna.Framework;

using StardustSandbox.Elements.Utilities;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Extensions;
using StardustSandbox.Randomness;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Elements.Liquids
{
    internal abstract class Liquid : Element
    {
        protected override void OnStep(ElementContext context)
        {
            foreach (Point belowPosition in ElementUtility.GetRandomSidePositions(context.Slot.Position, Direction.Down))
            {
                if (context.TrySetPosition(belowPosition))
                {
                    return;
                }

                if (context.TryGetSlot(belowPosition, out Slot belowSlot))
                {
                    SlotLayer belowLayer = belowSlot.GetLayer(context.Layer);

                    if (TrySwappingElements(context, belowPosition, belowLayer))
                    {
                        ElementUtility.NotifyFreeFallingFromAdjacentNeighbors(context, belowPosition);
                        context.SetElementState(belowPosition, context.Layer, ElementStates.FreeFalling);
                        return;
                    }

                    TryPerformConvection(context, belowPosition, belowLayer);
                }
            }

            UpdateHorizontalPosition(context);

            context.RemoveElementState(ElementStates.FreeFalling);
        }

        private bool TrySwappingElements(ElementContext context, Point position, SlotLayer belowLayer)
        {
            if (belowLayer.Element.Category == ElementCategory.Gas)
            {
                context.SwappingElements(position);
                return true;
            }
            else if (belowLayer.Element.Category == ElementCategory.Liquid && belowLayer.Element.DefaultDensity < this.DefaultDensity)
            {
                context.SwappingElements(position);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void TryPerformConvection(ElementContext context, Point position, SlotLayer belowLayer)
        {
            if (belowLayer.HasState(ElementStates.IsEmpty) ||
                belowLayer.Element.GetType() != GetType() ||
                belowLayer.Temperature <= context.SlotLayer.Temperature)
            {
                return;
            }

            context.SwappingElements(position);
        }

        private void UpdateHorizontalPosition(ElementContext context)
        {
            Point left = GetDispersionPosition(context, -1);
            Point right = GetDispersionPosition(context, 1);

            float leftDistance = context.Slot.Position.Distance(left);
            float rightDistance = context.Slot.Position.Distance(right);

            Point targetPosition = leftDistance == rightDistance
                ? SSRandom.Chance(50) ? left : right
                : leftDistance > rightDistance ? left : right;

            if (context.IsEmptySlotLayer(targetPosition, context.Layer))
            {
                context.SetPosition(targetPosition, context.Layer);
            }
            else
            {
                context.SwappingElements(context.Position, targetPosition, context.Layer);
            }
        }

        private Point GetDispersionPosition(ElementContext context, int direction)
        {
            Point dispersionPosition = context.Slot.Position;

            for (int i = 0; i < this.DefaultDispersionRate; i++)
            {
                Point nextPosition = new(dispersionPosition.X + direction, dispersionPosition.Y);

                Element nextElement = context.GetElement(nextPosition, context.Layer);

                if (context.IsEmptySlotLayer(nextPosition, context.Layer) || (nextElement != null && (nextElement.Category == ElementCategory.Liquid || nextElement.Category == ElementCategory.Gas)))
                {
                    dispersionPosition = nextPosition;
                }
                else
                {
                    break;
                }
            }

            return dispersionPosition;
        }
    }
}