using Microsoft.Xna.Framework;

using StardustSandbox.Core.Elements.Templates.Gases;
using StardustSandbox.Core.Elements.Templates.Liquids;
using StardustSandbox.Core.Elements.Utilities;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.World;

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
                        this.Context.SetElementFreeFalling(this.Context.Layer, position, true);
                        return;
                    }
                }

                this.Context.SetElementFreeFalling(this.Context.Layer, false);
            }
            else
            {
                if (TrySetPosition(new(this.Context.Slot.Position.X, this.Context.Slot.Position.Y + 1)))
                {
                    SElementUtility.NotifyFreeFallingFromAdjacentNeighbors(this.Context, belowPositions[0]);
                    this.Context.SetElementFreeFalling(this.Context.Layer, belowPositions[0], true);
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
            if (this.Context.TrySetPosition(this.Context.Layer, position))
            {
                return true;
            }

            if (this.Context.TryGetElement(this.Context.Layer, position, out ISElement value))
            {
                if ((value is SLiquid || value is SGas) && this.Context.TrySwappingElements(this.Context.Layer, position))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
