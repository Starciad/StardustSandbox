using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Elements.Gases;
using StardustSandbox.Elements.Liquids;
using StardustSandbox.Elements.Utilities;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Indexers;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal abstract class MovableSolid(Color referenceColor, ElementIndex index, Texture2D texture) : Solid(referenceColor, index, texture)
    {
        protected override void OnBehaviourStep()
        {
            if (this.Context.SlotLayer.FreeFalling)
            {
                foreach (Point belowPosition in ElementUtility.GetRandomSidePositions(this.Context.Slot.Position, Direction.Down))
                {
                    if (TrySetPosition(belowPosition))
                    {
                        ElementUtility.NotifyFreeFallingFromAdjacentNeighbors(this.Context, belowPosition);
                        this.Context.SetElementFreeFalling(belowPosition, this.Context.Layer, true);
                        return;
                    }
                }

                this.Context.SetElementFreeFalling(this.Context.Layer, false);
            }
            else
            {
                Point below = new(this.Context.Slot.Position.X, this.Context.Slot.Position.Y + 1);
                if (TrySetPosition(below))
                {
                    ElementUtility.NotifyFreeFallingFromAdjacentNeighbors(this.Context, below);
                    this.Context.SetElementFreeFalling(below, this.Context.Layer, true);
                    return;
                }
                else
                {
                    this.Context.SetElementFreeFalling(this.Context.Layer, false);
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
                if (value is Liquid liquid)
                {
                    if (this.DefaultDensity > liquid.DefaultDensity && this.Context.TrySwappingElements(position))
                    {
                        return true;
                    }
                }

                if (value is Gas && this.Context.TrySwappingElements(position))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
