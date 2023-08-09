using Microsoft.Xna.Framework;

using PixelDust.Core.Utilities;

namespace PixelDust.Core.Elements
{
    public abstract class PMovableSolid : PSolid
    {
        internal override void OnBehaviourStep()
        {
            int direction = PRandom.Range(0, 101) < 50 ? 1 : -1;
            Vector2[] targets = new Vector2[]
            {
                new(Context.Position.X                   , Context.Position.Y + 1),
                new(Context.Position.X + direction       , Context.Position.Y + 1),
                new(Context.Position.X + direction * -1  , Context.Position.Y + 1),
            };

            foreach (Vector2 targetPos in targets)
            {
                if (Context.IsEmpty(targetPos))
                    if (Context.TrySetPosition(targetPos)) return;

                if (Context.TryGetElement(targetPos, out PElement value))
                {
                    if (value is PLiquid)
                        if (Context.TrySwitchPosition(Context.Position, targetPos)) return;
                }
            }
        }
    }
}
