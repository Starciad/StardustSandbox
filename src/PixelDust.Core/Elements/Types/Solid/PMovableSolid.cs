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

            Vector2 down = new(PElementContext.Position.X, PElementContext.Position.Y + 1);
            Vector2[] sides = new Vector2[]
            {
                new(PElementContext.Position.X + direction     , PElementContext.Position.Y),
                new(PElementContext.Position.X + direction * -1, PElementContext.Position.Y),
            };

            if (TrySetPosition(down)) { return; }

            foreach (Vector2 targetPos in sides)
            {
                if (PElementContext.IsEmpty(targetPos))
                {
                    if (TrySetPosition(new(targetPos.X, targetPos.Y + 1))) { return; }
                }
            }
        }

        private static bool TrySetPosition(Vector2 pos)
        {
            if (PElementContext.IsEmpty(pos))
                if (PElementContext.TrySetPosition(pos)) return true;

            if (PElementContext.TryGetElement(pos, out PElement value))
            {
                if (value is PLiquid)
                    if (PElementContext.TrySwitchPosition(PElementContext.Position, pos)) return true;
            }

            return false;
        }
    }
}
