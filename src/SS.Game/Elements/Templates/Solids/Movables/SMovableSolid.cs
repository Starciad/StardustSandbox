using Microsoft.Xna.Framework;

using StardustSandbox.Game.Elements.Templates.Liquids;
using StardustSandbox.Game.General;

namespace StardustSandbox.Game.Elements.Templates.Solids.Movables
{
    public abstract class SMovableSolid(SGame gameInstance) : SSolid(gameInstance)
    {
        public override void OnBehaviourStep()
        {
            int direction = SRandom.Range(0, 101) < 50 ? 1 : -1;
            Point down = new(this.Context.Position.X, this.Context.Position.Y + 1);

            if (this.Context.Slot.FreeFalling)
            {
                Point[] sides = [
                    new(this.Context.Position.X + direction, this.Context.Position.Y),
                    new(this.Context.Position.X + (direction * -1), this.Context.Position.Y),
                ];

                if (TrySetPosition(down))
                {
                    NotifyFreeFallingFromAdjacentNeighbors(down);
                    this.Context.SetElementFreeFalling(down, true);
                    return;
                }

                for (int i = 0; i < sides.Length; i++)
                {
                    Point sidePos = sides[i];
                    Point finalPos = new(sidePos.X, sidePos.Y + 1);

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
                if (TrySetPosition(down))
                {
                    NotifyFreeFallingFromAdjacentNeighbors(down);
                    this.Context.SetElementFreeFalling(down, true);
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
    }
}
