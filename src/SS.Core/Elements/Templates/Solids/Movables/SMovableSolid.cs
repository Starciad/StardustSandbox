using Microsoft.Xna.Framework;

using StardustSandbox.Core.Elements.Templates.Gases;
using StardustSandbox.Core.Elements.Templates.Liquids;
using StardustSandbox.Core.Elements.Utilities;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.Core.Elements.Templates.Solids.Movables
{
    public abstract class SMovableSolid(ISGame gameInstance) : SSolid(gameInstance)
    {
        protected override void OnBehaviourStep()
        {
            Point[] belowPositions = SElementUtility.GetRandomSidePositions(this.Context.Slot.Position, SDirection.Down);

            if (this.Context.SlotLayer.FreeFalling)
            {
                for (int i = 0; i < belowPositions.Length; i++)
                {
                    Point position = belowPositions[i];

                    if (TrySetPosition(position))
                    {
                        SElementUtility.NotifyFreeFallingFromAdjacentNeighbors(this.Context, position);
                        this.Context.SetElementFreeFalling(position, this.Context.Layer, true);
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
