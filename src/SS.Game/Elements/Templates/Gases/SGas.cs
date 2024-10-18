using Microsoft.Xna.Framework;

using StardustSandbox.Game.Elements.Templates.Liquids;
using StardustSandbox.Game.Elements.Templates.Solids.Movables;
using StardustSandbox.Game.Mathematics;

namespace StardustSandbox.Game.Elements.Templates.Gases
{
    public enum GasSpreadingType
    {
        Up,
        Spread
    }

    public abstract class SGas(SGame gameInstance) : SElement(gameInstance)
    {
        public GasSpreadingType SpreadingType { get; protected set; }

        public override void OnBehaviourStep()
        {
            switch (this.SpreadingType)
            {
                case GasSpreadingType.Up:
                    UpSpreadingTypeUpdate();
                    break;

                case GasSpreadingType.Spread:
                    break;

                default:
                    break;
            }
        }

        private void UpSpreadingTypeUpdate()
        {
            int direction = SRandomMath.Range(0, 101) < 50 ? 1 : -1;
            Point[] targets =
            [
                new Point(this.Context.Position.X, this.Context.Position.Y - 1),
                new Point(this.Context.Position.X + direction, this.Context.Position.Y - 1),
                new Point(this.Context.Position.X + (direction * -1), this.Context.Position.Y - 1),
            ];

            foreach (Point targetPos in targets)
            {
                if (this.Context.IsEmptyElementSlot(targetPos))
                {
                    if (this.Context.TrySetPosition(targetPos))
                    {
                        return;
                    }
                }

                if (this.Context.TryGetElement(targetPos, out SElement value))
                {
                    if ((value is SLiquid || value is SMovableSolid) && this.Context.TrySwappingElements(targetPos))
                    {
                        return;
                    }
                }
            }

            for (int i = 0; i < this.DefaultDispersionRate; i++)
            {
                if (!this.Context.IsEmptyElementSlot(new(this.Context.Position.X + direction, this.Context.Position.Y)) &&
                    !this.Context.IsEmptyElementSlot(new(this.Context.Position.X + direction, this.Context.Position.Y - 1)))
                {
                    break;
                }

                this.Context.SetPosition(new(this.Context.Position.X + direction, this.Context.Position.Y));
            }
        }
    }
}
