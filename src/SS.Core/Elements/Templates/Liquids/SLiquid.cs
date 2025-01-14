using Microsoft.Xna.Framework;

using StardustSandbox.Core.Elements.Templates.Gases;
using StardustSandbox.Core.Elements.Utilities;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Elements;

namespace StardustSandbox.Core.Elements.Templates.Liquids
{
    public abstract class SLiquid : SElement
    {
        public SLiquid(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.defaultDensity = 1000;
        }

        protected override void OnBehaviourStep()
        {
            foreach (Point belowPosition in SElementUtility.GetRandomSidePositions(this.Context.Slot.Position, SDirection.Down))
            {
                if (TrySetPosition(belowPosition))
                {
                    SElementUtility.NotifyFreeFallingFromAdjacentNeighbors(this.Context, belowPosition);
                    this.Context.SetElementFreeFalling(belowPosition, this.Context.Layer, true);
                    return;
                }
            }

            SElementUtility.UpdateHorizontalPosition(this.Context, this.DefaultDispersionRate);
            this.Context.SetElementFreeFalling(this.Context.Layer, false);
        }

        private bool TrySetPosition(Point position)
        {
            if (this.Context.TrySetPosition(position))
            {
                return true;
            }

            if (this.Context.TryGetElement(position, this.Context.Layer, out ISElement value))
            {
                if (value is SGas && this.Context.TrySwappingElements(position))
                {
                    return true;
                }

                if (value is SLiquid)
                {
                    if (value.DefaultDensity < this.DefaultDensity && this.Context.TrySwappingElements(position))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}