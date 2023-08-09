using Microsoft.Xna.Framework;

using PixelDust.Core.Utilities;

namespace PixelDust.Core.Elements
{
    public enum GasSpreadingType
    {
        Up,
        Spread
    }

    public abstract class PGas : PElement
    {
        public GasSpreadingType SpreadingType { get; protected set; }

        internal override void OnBehaviourStep()
        {
            switch (SpreadingType)
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
            Vector2[] targets = new Vector2[]
            {
                    new(Context.Position.X                   , Context.Position.Y - 1),
                    new(Context.Position.X + direction       , Context.Position.Y - 1),
                    new(Context.Position.X + direction * -1, Context.Position.Y - 1),
            };

            foreach (Vector2 targetPos in targets)
            {
                if (Context.IsEmpty(targetPos))
                    if (Context.TrySetPosition(targetPos)) return;

                if (Context.TryGetElement(targetPos, out PElement value))
                {
                    if (value is PLiquid ||
                        value is PMovableSolid)
                        if (Context.TrySwitchPosition(Context.Position, targetPos)) return;
                }
            }

            for (int i = 0; i < DefaultDispersionRate; i++)
            {
                if (!Context.IsEmpty(new(Context.Position.X + direction, Context.Position.Y)) &&
                    !Context.IsEmpty(new(Context.Position.X + direction, Context.Position.Y - 1)))
                    break;

                Context.TrySetPosition(new(Context.Position.X + direction, Context.Position.Y));
            }
        }
    }
}
