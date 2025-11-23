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
        protected override void OnStep()
        {
            foreach (Point belowPosition in ElementUtility.GetRandomSidePositions(this.Context.Slot.Position, Direction.Down))
            {
                if (this.Context.TrySetPosition(belowPosition))
                {
                    return;
                }

                if (this.Context.TryGetSlot(belowPosition, out Slot belowSlot))
                {
                    SlotLayer belowLayer = belowSlot.GetLayer(this.Context.Layer);

                    if (TrySwappingElements(belowPosition, belowLayer))
                    {
                        ElementUtility.NotifyFreeFallingFromAdjacentNeighbors(this.Context, belowPosition);
                        this.Context.SetElementState(belowPosition, this.Context.Layer, ElementStates.FreeFalling);
                        return;
                    }

                    TryPerformConvection(belowPosition, belowLayer);
                }
            }

            UpdateHorizontalPosition();

            this.Context.RemoveElementState(ElementStates.FreeFalling);
        }

        private bool TrySwappingElements(Point position, SlotLayer belowLayer)
        {
            if (belowLayer.Element.Category == ElementCategory.Gas)
            {
                this.Context.SwappingElements(position);
                return true;
            }
            else if (belowLayer.Element.Category == ElementCategory.Liquid && belowLayer.Element.DefaultDensity < this.DefaultDensity)
            {
                this.Context.SwappingElements(position);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void TryPerformConvection(Point position, SlotLayer belowLayer)
        {
            if (belowLayer.HasState(ElementStates.IsEmpty) ||
                belowLayer.Element.GetType() != GetType() ||
                belowLayer.Temperature <= this.Context.SlotLayer.Temperature)
            {
                return;
            }

            this.Context.SwappingElements(position);
        }

        private void UpdateHorizontalPosition()
        {
            Point left = GetDispersionPosition(-1);
            Point right = GetDispersionPosition(1);

            float leftDistance = this.Context.Slot.Position.Distance(left);
            float rightDistance = this.Context.Slot.Position.Distance(right);

            Point targetPosition = leftDistance == rightDistance
                ? SSRandom.Chance(50) ? left : right
                : leftDistance > rightDistance ? left : right;

            if (this.Context.IsEmptySlotLayer(targetPosition, this.Context.Layer))
            {
                this.Context.SetPosition(targetPosition, this.Context.Layer);
            }
            else
            {
                this.Context.SwappingElements(this.Context.Position, targetPosition, this.Context.Layer);
            }
        }

        private Point GetDispersionPosition(int direction)
        {
            Point dispersionPosition = this.Context.Slot.Position;

            for (int i = 0; i < this.DefaultDispersionRate; i++)
            {
                Point nextPosition = new(dispersionPosition.X + direction, dispersionPosition.Y);

                Element nextElement = this.Context.GetElement(nextPosition, this.Context.Layer);

                if (this.Context.IsEmptySlotLayer(nextPosition, this.Context.Layer) || (nextElement != null && (nextElement.Category == ElementCategory.Liquid || nextElement.Category == ElementCategory.Gas)))
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