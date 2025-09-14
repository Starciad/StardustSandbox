using Microsoft.Xna.Framework;

using StardustSandbox.Core.Elements.Templates.Gases;
using StardustSandbox.Core.Elements.Templates.Liquids;
using StardustSandbox.Core.Elements.Utilities;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Elements;

namespace StardustSandbox.Core.Elements.Templates.Solids.Movables
{
    public abstract class SMovableSolid(ISGame gameInstance, string identifier) : SSolid(gameInstance, identifier)
    {
        protected override void OnBehaviourStep()
        {
            if (this.Context.SlotLayer.FreeFalling)
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

                this.Context.SetElementFreeFalling(this.Context.Layer, false);
            }
            else
            {
                Point below = new(this.Context.Slot.Position.X, this.Context.Slot.Position.Y + 1);
                if (TrySetPosition(below))
                {
                    SElementUtility.NotifyFreeFallingFromAdjacentNeighbors(this.Context, below);
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

            if (this.Context.TryGetElement(position, this.Context.Layer, out ISElement value))
            {
                if (value is SLiquid liquid)
                {
                    if (this.DefaultDensity > liquid.DefaultDensity && this.Context.TrySwappingElements(position))
                    {
                        return true;
                    }
                }

                if (value is SGas && this.Context.TrySwappingElements(position))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
