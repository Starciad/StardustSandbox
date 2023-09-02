using PixelDust.Core.Mathematics;
using PixelDust.Core.Utilities;

namespace PixelDust.Core.Elements
{
    /// <summary>
    /// Base class for defining movable solid elements in PixelDust.
    /// </summary>
    public abstract class PMovableSolid : PSolid
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

            if (TrySetPosition(down)) { return; }

            foreach (Vector2Int side in sides)
            {
                if (TrySetPosition(new(side.X, side.Y + 1))) { return; }
            }
        }

        private bool TrySetPosition(Vector2Int pos)
        {
            if (Context.TrySetPosition(pos)) return true;

            if (Context.TryGetElement(pos, out PElement value))
            {
                if (value is PLiquid)
                    if (Context.TrySwitchPosition(Context.Position, pos)) return true;
            }

            return false;
        }
    }
}
