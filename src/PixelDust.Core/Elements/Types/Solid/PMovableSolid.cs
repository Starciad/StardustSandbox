using Microsoft.Xna.Framework;

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

            Vector2 down = new(Context.Position.X, Context.Position.Y + 1);
            Vector2[] sides = new Vector2[]
            {
                new(Context.Position.X + direction     , Context.Position.Y),
                new(Context.Position.X + direction * -1, Context.Position.Y),
            };

            if (TrySetPosition(down)) { return; }

            foreach (Vector2 targetPos in sides)
            {
                if (Context.IsEmpty(targetPos))
                {
                    if (TrySetPosition(new(targetPos.X, targetPos.Y + 1))) { return; }
                }
            }
        }

        private bool TrySetPosition(Vector2 pos)
        {
            if (Context.IsEmpty(pos))
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
