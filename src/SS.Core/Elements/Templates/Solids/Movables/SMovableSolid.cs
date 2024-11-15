using Microsoft.Xna.Framework;

using StardustSandbox.Core.Elements.Templates.Liquids;
using StardustSandbox.Core.Elements.Utilities;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Elements;

namespace StardustSandbox.Core.Elements.Templates.Solids.Movables
{
    public abstract class SMovableSolid(ISGame gameInstance) : SSolid(gameInstance)
    {
        protected override void OnBehaviourStep()
        {
            Point[] belowPositions = SElementUtility.GetRandomSidePositions(this.Context.Position, SDirection.Down);

            if (this.Context.Slot.FreeFalling)
            {
                for (int i = 0; i < belowPositions.Length; i++)
                {
                    Point sidePos = belowPositions[i];
                    Point finalPos = new(sidePos.X, sidePos.Y);

                    if (TrySetPosition(finalPos))
                    {
                        SElementUtility.NotifyFreeFallingFromAdjacentNeighbors(this.Context, finalPos);
                        this.Context.SetElementFreeFalling(finalPos, true);
                        return;
                    }
                }

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
                    this.Context.SetElementFreeFalling(false);
                    return;
                }
            }
        }

        private bool TrySetPosition(Point pos)
        {
            if (this.Context.TrySetPosition(pos))
            {
                return true;
            }

            if (this.Context.TryGetElement(pos, out ISElement value))
            {
                if (value is SLiquid && this.Context.TrySwappingElements(pos))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
