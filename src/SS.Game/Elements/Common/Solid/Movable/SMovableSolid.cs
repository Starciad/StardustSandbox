using Microsoft.Xna.Framework;

using StardustSandbox.Game.Elements.Common.Liquid;
using StardustSandbox.Game.General;

namespace StardustSandbox.Game.Elements.Common.Solid.Movable
{
    public abstract class SMovableSolid : SSolid
    {
        public SMovableSolid(SGame gameInstance) : base(gameInstance)
        {

        }

        public override void OnBehaviourStep()
        {
            int direction = SRandom.Range(0, 101) < 50 ? 1 : -1;

            Point down = new(this.Context.Position.X, this.Context.Position.Y + 1);
            Point[] sides =
            [
                new(this.Context.Position.X + direction, this.Context.Position.Y),
                new(this.Context.Position.X + (direction * -1), this.Context.Position.Y),
            ];

            if (TrySetPosition(down))
            { return; }

            foreach (Point side in sides)
            {
                if (TrySetPosition(new(side.X, side.Y + 1)))
                { return; }
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
    }
}
