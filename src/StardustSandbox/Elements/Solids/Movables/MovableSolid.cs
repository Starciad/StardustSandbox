using Microsoft.Xna.Framework;

using StardustSandbox.Elements.Utilities;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal abstract class MovableSolid : Solid
    {
        protected override void OnStep(in ElementContext context)
        {
            if (context.SlotLayer.HasState(ElementStates.FreeFalling))
            {
                foreach (Point belowPosition in ElementUtility.GetRandomSidePositions(context.Slot.Position, Direction.Down))
                {
                    if (TrySetPosition(context, belowPosition))
                    {
                        ElementUtility.NotifyFreeFallingFromAdjacentNeighbors(context, belowPosition);
                        context.SetElementState(belowPosition, ElementStates.FreeFalling);
                        return;
                    }
                }

                context.RemoveElementState(ElementStates.FreeFalling);
            }
            else
            {
                Point belowPosition = new(context.Slot.Position.X, context.Slot.Position.Y + 1);

                if (TrySetPosition(context, belowPosition))
                {
                    ElementUtility.NotifyFreeFallingFromAdjacentNeighbors(context, belowPosition);
                    context.SetElementState(belowPosition, ElementStates.FreeFalling);
                    return;
                }
                else
                {
                    context.RemoveElementState(ElementStates.FreeFalling);
                    return;
                }
            }
        }

        private bool TrySetPosition(in ElementContext context, Point position)
        {
            if (context.TrySetPosition(position))
            {
                return true;
            }

            if (context.TryGetElement(position, context.Layer, out Element value))
            {
                if (value.Category == ElementCategory.Liquid)
                {
                    if (this.DefaultDensity > value.DefaultDensity && context.TrySwappingElements(position))
                    {
                        return true;
                    }
                }

                if (value.Category == ElementCategory.Gas && context.TrySwappingElements(position))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
