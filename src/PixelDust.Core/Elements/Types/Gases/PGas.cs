using Microsoft.Xna.Framework;

using PixelDust.Core.Utilities;

namespace PixelDust.Core.Elements
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
                    new(PElementContext.Position.X                   , PElementContext.Position.Y - 1),
                    new(PElementContext.Position.X + direction       , PElementContext.Position.Y - 1),
                    new(PElementContext.Position.X + direction * -1, PElementContext.Position.Y - 1),
            };

            foreach (Vector2 targetPos in targets)
            {
                if (PElementContext.IsEmpty(targetPos))
                    if (PElementContext.TrySetPosition(targetPos)) return;

                if (PElementContext.TryGetElement(targetPos, out PElement value))
                {
                    if (value is PLiquid ||
                        value is PMovableSolid)
                        if (PElementContext.TrySwitchPosition(PElementContext.Position, targetPos)) return;
                }
            }

            for (int i = 0; i < DefaultDispersionRate; i++)
            {
                if (!PElementContext.IsEmpty(new(PElementContext.Position.X + direction, PElementContext.Position.Y)) &&
                    !PElementContext.IsEmpty(new(PElementContext.Position.X + direction, PElementContext.Position.Y - 1)))
                    break;

                PElementContext.TrySetPosition(new(PElementContext.Position.X + direction, PElementContext.Position.Y));
            }
        }
    }
}
