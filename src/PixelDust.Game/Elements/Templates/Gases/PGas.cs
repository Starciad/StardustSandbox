using PixelDust.Game.Elements.Templates.Liquid;
using PixelDust.Game.Elements.Templates.Solid;
using PixelDust.Game.Mathematics;
using PixelDust.Game.Utilities;

namespace PixelDust.Game.Elements.Templates.Gases
{
    public enum GasSpreadingType
    {
        Up,
        Spread
    }

    /// <summary>
    /// Base class for defining gases elements in PixelDust.
    /// </summary>
    public abstract class PGas : PElement
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
            int direction = PRandom.Range(0, 101) < 50 ? 1 : -1;
            Vector2Int[] targets =
            [
                new Vector2Int(this.Context.Position.X, this.Context.Position.Y - 1),
                new Vector2Int(this.Context.Position.X + direction, this.Context.Position.Y - 1),
                new Vector2Int(this.Context.Position.X + (direction * -1), this.Context.Position.Y - 1),
            ];

            foreach (Vector2Int targetPos in targets)
            {
                if (this.Context.IsEmptyElementSlot(targetPos))
                {
                    if (this.Context.TrySetPosition(targetPos))
                    {
                        return;
                    }
                }

                if (this.Context.TryGetElement(targetPos, out PElement value))
                {
                    if (value is PLiquid ||
                        value is PMovableSolid)
                    {
                        if (this.Context.TrySwappingElements(targetPos))
                        {
                            return;
                        }
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
