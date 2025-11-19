using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Elements.Gases;
using StardustSandbox.Elements.Utilities;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Indexers;
using StardustSandbox.Extensions;
using StardustSandbox.Randomness;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Elements.Liquids
{
    internal abstract class Liquid : Element
    {
        internal Liquid(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.defaultDensity = 1000;
        }

        protected override void OnBehaviourStep()
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
                        this.Context.SetElementFreeFalling(belowPosition, this.Context.Layer, true);
                        return;
                    }

                    TryPerformConvection(belowPosition, belowLayer);
                }
            }

            UpdateHorizontalPosition();

            this.Context.SetElementFreeFalling(this.Context.Layer, false);
        }

        private bool TrySwappingElements(Point position, SlotLayer belowLayer)
        {
            switch (belowLayer.Element)
            {
                case Gas:
                    this.Context.SwappingElements(position);
                    return true;

                case Liquid:
                    if (belowLayer.Element.DefaultDensity < this.DefaultDensity)
                    {
                        this.Context.SwappingElements(position);
                        return true;
                    }

                    break;
            }

            return false;
        }

        private void TryPerformConvection(Point position, SlotLayer belowLayer)
        {
            if (belowLayer.IsEmpty ||
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
                this.Context.SwappingElements(targetPosition, this.Context.Layer);
            }
        }

        private Point GetDispersionPosition(int direction)
        {
            Point dispersionPosition = this.Context.Slot.Position;

            for (int i = 0; i < this.defaultDispersionRate; i++)
            {
                Point nextPosition = new(dispersionPosition.X + direction, dispersionPosition.Y);

                if (this.Context.IsEmptySlotLayer(nextPosition, this.Context.Layer) ||
                    this.Context.GetElement(nextPosition, this.Context.Layer) is Liquid or Gas)
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