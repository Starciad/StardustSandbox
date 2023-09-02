using PixelDust.Core.Mathematics;
using PixelDust.Core.Utilities;

namespace PixelDust.Core.Elements
{
    /// <summary>
    /// Base class for defining liquid elements in PixelDust.
    /// </summary>
    public abstract class PLiquid : PElement
    {
        internal override void OnBehaviourStep()
        {
            int direction = PRandom.Range(0, 101) < 50 ? 1 : -1;

            Vector2Int down = new(Context.Position.X, Context.Position.Y + 1);
            Vector2Int[] sides = new Vector2Int[]
            {
                new(Context.Position.X + direction     , Context.Position.Y),
                new(Context.Position.X + direction * -1, Context.Position.Y),
            };

            if (Context.TrySetPosition(down))
                return;

            foreach (Vector2Int side in sides)
                if (Context.TrySetPosition(new(side.X, side.Y + 1))) { return; }

            HorizontalMovementUpdate(direction);
        }

        private void HorizontalMovementUpdate(int direction)
        {
            int lCount = 0, rCount = 0;
            bool lCountBreak = false, rCountBreak = false;

            // Left (<) && Right (>)
            for (int i = 0; i < DefaultDispersionRate; i++)
            {
                if (lCountBreak && rCountBreak)
                    break;

                if (!lCountBreak && Context.IsEmpty(new(Context.Position.X - (i + 1), Context.Position.Y)))
                {
                    lCount++;
                }
                else
                {
                    lCountBreak = true;
                }

                if (!rCountBreak && Context.IsEmpty(new(Context.Position.X + (i + 1), Context.Position.Y)))
                {
                    rCount++;
                }
                else
                {
                    rCountBreak = true;
                }
            }

            // Set new position
            int tCount = int.Max(lCount, rCount);

            Vector2Int lPosition = new(Context.Position.X - tCount, Context.Position.Y);
            Vector2Int rPosition = new(Context.Position.X + tCount, Context.Position.Y);

            if (tCount.Equals(lCount) && tCount.Equals(rCount))
            {
                if (direction.Equals(-1))
                {
                    if (Context.TrySetPosition(lPosition)) { return; }
                }

                if (direction.Equals(1))
                {
                    if (Context.TrySetPosition(rPosition)) { return; }
                }
            }

            if (tCount.Equals(lCount))
            {
                if (Context.TrySetPosition(lPosition)) { return; }
            }

            if (tCount.Equals(rCount))
            {
                if (Context.TrySetPosition(rPosition)) { return; }
            }
        }
    }
}