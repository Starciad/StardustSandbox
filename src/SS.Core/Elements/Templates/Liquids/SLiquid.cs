using Microsoft.Xna.Framework;

using StardustSandbox.Core.Elements.Templates.Gases;
using StardustSandbox.Core.Elements.Utilities;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics;

namespace StardustSandbox.Core.Elements.Templates.Liquids
{
    public abstract class SLiquid(ISGame gameInstance) : SElement(gameInstance)
    {
        protected override void OnBehaviourStep()
        {
            Point[] belowPositions = SElementUtility.GetRandomSidePositions(this.Context.Position, SDirection.Down);

            if (this.Context.Slot.FreeFalling)
            {
                for (int i = 0; i < belowPositions.Length; i++)
                {
                    Point position = belowPositions[i];

                    if (TrySetPosition(position))
                    {
                        SElementUtility.NotifyFreeFallingFromAdjacentNeighbors(this.Context, position);
                        this.Context.SetElementFreeFalling(position, true);
                        return;
                    }
                }

                SElementUtility.UpdateHorizontalPosition(this.Context, this.DefaultDispersionRate);
                this.Context.SetElementFreeFalling(false);
            }
            else
            {
                if (TrySetPosition(new(this.Context.Position.X, this.Context.Position.Y + 1)))
                {
                    SElementUtility.NotifyFreeFallingFromAdjacentNeighbors(this.Context, belowPositions[0]);
                    this.Context.SetElementFreeFalling(belowPositions[0], true);
                    return;
                }
                else
                {
                    SElementUtility.UpdateHorizontalPosition(this.Context, this.DefaultDispersionRate);
                    this.Context.SetElementFreeFalling(false);
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

            if (this.Context.TryGetElement(position, out ISElement value))
            {
                if (value is SGas && this.Context.TrySwappingElements(position))
                {
                    return true;
                }
            }

            return false;
        }
    }
}