using Microsoft.Xna.Framework;

using PixelDust.Core.Utilities;

using System;

namespace PixelDust.Core.Elements
{
    public abstract class PLiquid : PElement
    {
        internal override void OnBehaviourStep()
        {
            // Creation of quick access positions in the context of this current liquid
            int direction = PRandom.Range(0, 101) < 50 ? 1 : -1;
            Vector2[] targets = new Vector2[]
            {
                new(Context.Position.X                   , Context.Position.Y + 1),
                new(Context.Position.X + direction       , Context.Position.Y + 1),
                new(Context.Position.X + direction * -1  , Context.Position.Y + 1),
            };

            // Down
            // Liquido tries to move to target positions
            foreach (Vector2 targetPos in targets)
            {
                if (Context.IsEmpty(targetPos))
                    if (Context.TrySetPosition(targetPos)) return;
            }

            // Horizontal
            Vector2[] horizontal = new Vector2[]
            {
                new(Context.Position.X + direction     , Context.Position.Y),
                new(Context.Position.X + direction * -1, Context.Position.Y),
            };

            foreach (Vector2 targetPos in horizontal)
            {
                if (Context.IsEmpty(targetPos))
                    if (Context.TrySetPosition(targetPos)) return;
            }
        }
    }
}