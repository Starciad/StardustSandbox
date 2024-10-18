using Microsoft.Xna.Framework;

using StardustSandbox.Game.Elements.Templates.Liquids;
using StardustSandbox.Game.Mathematics;

namespace StardustSandbox.Game.Elements.Templates.Solids.Movables
{
    public abstract class SMovableSolid(SGame gameInstance) : SSolid(gameInstance)
    {
        public override void OnBehaviourStep()
        {
            Point[] belowPositions = GetRandomBelowPositions(this.Context.Position);

            if (this.Context.Slot.FreeFalling)
            {
                for (int i = 0; i < belowPositions.Length; i++)
                {
                    Point sidePos = belowPositions[i];
                    Point finalPos = new(sidePos.X, sidePos.Y);

                    if (TrySetPosition(finalPos))
                    {
                        NotifyFreeFallingFromAdjacentNeighbors(finalPos);
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
                    NotifyFreeFallingFromAdjacentNeighbors(belowPositions[0]);
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

            if (this.Context.TryGetElement(pos, out SElement value))
            {
                if (value is SLiquid)
                {
                    if (this.Context.TrySwappingElements(pos))
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }

        private void NotifyFreeFallingFromAdjacentNeighbors(Point position)
        {
            this.Context.SetElementFreeFalling(new(position.X, position.Y - 1), true);
            this.Context.SetElementFreeFalling(new(position.X, position.Y + 1), true);
            this.Context.SetElementFreeFalling(new(position.X - 1, position.Y), true);
            this.Context.SetElementFreeFalling(new(position.X + 1, position.Y), true);
        }

        private static Point[] GetRandomBelowPositions(Point targetPosition)
        {
            int rDirection = SRandomMath.Chance(50, 100) ? 1 : -1;

            return [
                new(targetPosition.X, targetPosition.Y + 1),
                new(targetPosition.X + (rDirection), targetPosition.Y + 1),
                new(targetPosition.X + (rDirection * -1), targetPosition.Y + 1),
            ];
        }
    }
}
