using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Elements.Utilities;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal abstract class MovableSolid : Solid
    {
        internal MovableSolid(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.category = ElementCategory.MovableSolid;
        }

        protected override void OnStep()
        {
            if (this.Context.SlotLayer.HasState(ElementStates.FreeFalling))
            {
                foreach (Point belowPosition in ElementUtility.GetRandomSidePositions(this.Context.Slot.Position, Direction.Down))
                {
                    if (TrySetPosition(belowPosition))
                    {
                        ElementUtility.NotifyFreeFallingFromAdjacentNeighbors(this.Context, belowPosition);
                        this.Context.SetElementState(belowPosition, ElementStates.FreeFalling);
                        return;
                    }
                }

                this.Context.RemoveElementState(ElementStates.FreeFalling);
            }
            else
            {
                Point belowPosition = new(this.Context.Slot.Position.X, this.Context.Slot.Position.Y + 1);

                if (TrySetPosition(belowPosition))
                {
                    ElementUtility.NotifyFreeFallingFromAdjacentNeighbors(this.Context, belowPosition);
                    this.Context.SetElementState(belowPosition, ElementStates.FreeFalling);
                    return;
                }
                else
                {
                    this.Context.RemoveElementState(ElementStates.FreeFalling);
                    return;
                }
            }
        }

        private bool TrySetPosition(Point position)
        {
            if (this.Context.TrySetPosition(position))
            {
                return true;
            }

            if (this.Context.TryGetElement(position, this.Context.Layer, out Element value))
            {
                if (value.Category == ElementCategory.Liquid)
                {
                    if (this.DefaultDensity > value.DefaultDensity && this.Context.TrySwappingElements(position))
                    {
                        return true;
                    }
                }

                if (value.Category == ElementCategory.Gas && this.Context.TrySwappingElements(position))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
