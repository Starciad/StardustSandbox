using Microsoft.Xna.Framework;

using StardustSandbox.Elements.Utilities;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal abstract class MovableSolid : Solid
    {
        protected override void OnStep(ElementContext context)
        {
            if (context.SlotLayer.HasState(ElementStates.IsFalling))
            {
                foreach (Point belowPosition in ElementUtility.GetRandomSidePositions(context.Slot.Position, Direction.Down))
                {
                    if (TrySetPosition(context, belowPosition))
                    {
                        ElementUtility.NotifyFreeFallingFromAdjacentNeighbors(context, belowPosition);
                        context.SetElementState(belowPosition, ElementStates.IsFalling);
                        return;
                    }
                }

                context.RemoveElementState(ElementStates.IsFalling);
            }
            else
            {
                Point belowPosition = new(context.Slot.Position.X, context.Slot.Position.Y + 1);

                if (TrySetPosition(context, belowPosition))
                {
                    ElementUtility.NotifyFreeFallingFromAdjacentNeighbors(context, belowPosition);
                    context.SetElementState(belowPosition, ElementStates.IsFalling);
                    return;
                }
                else
                {
                    context.RemoveElementState(ElementStates.IsFalling);
                    return;
                }
            }
        }

        private bool TrySetPosition(ElementContext context, Point position)
        {
            if (context.TrySetPosition(position))
            {
                return true;
            }

            if (context.TryGetElement(position, context.Layer, out Element value))
            {
                switch (value.Category)
                {
                    case ElementCategory.Liquid:
                        if (this.DefaultDensity > value.DefaultDensity && context.TrySwappingElements(position))
                        {
                            return true;
                        }

                        break;

                    case ElementCategory.Gas:
                        if (context.TrySwappingElements(position))
                        {
                            return true;
                        }

                        break;

                    default:
                        break;
                }
            }

            return false;
        }
    }
}
