using Microsoft.Xna.Framework;

using StardustSandbox.Core.Elements.Templates.Liquids;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Mathematics;

namespace StardustSandbox.Core.Elements.Templates.Gases
{
    public enum GasSpreadingType
    {
        Up,
        Spread
    }

    public abstract class SGas(ISGame gameInstance) : SElement(gameInstance)
    {
        public GasSpreadingType SpreadingType { get; protected set; }

        protected override void OnBehaviourStep()
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

            for (int i = 0; i < targets.Length; i++)
            {
                Point targetPos = targets[i];

                if (this.Context.IsEmptyElementSlot(targetPos))
                {
                    if (this.Context.TrySetPosition(targetPos))
                    {
                        return;
                    }
                }

                if (this.Context.TryGetElement(targetPos, out ISElement value))
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
