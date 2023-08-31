using Microsoft.Xna.Framework;

using PixelDust.Core.Utilities;

using System;

namespace PixelDust.Core.Elements
{
    /// <summary>
    /// Base class for defining liquid elements in PixelDust.
    /// </summary>
    public abstract class PLiquid : PElement
    {
        internal override void OnBehaviourStep()
        {
            // Creation of quick access positions in the context of this current liquid
            int direction = PRandom.Range(0, 101) < 50 ? 1 : -1;
            Vector2[] targets = new Vector2[]
            {
                new(Context.Position.X                  , Context.Position.Y + 1),
                new(Context.Position.X + direction      , Context.Position.Y + 1),
                new(Context.Position.X + direction * -1 , Context.Position.Y + 1),
            };

            // Down
            // Liquido tries to move to target positions
            foreach (Vector2 targetPos in targets)
            {
                if (Context.IsEmpty(targetPos))
                    if (Context.TrySetPosition(targetPos)) return;
            }

            HorizontalMovement();
        }

        private void HorizontalMovement()
        {
            int targetDirection, targetDistance;
            int lDistance = 0, rDistance = 0;

            // Check left side (<)
            for (int i = 0; i < DefaultDispersionRate; i++)
            {
                if (Context.IsEmpty(new(Context.Position.X - (i + 1), Context.Position.Y)))
                    lDistance++;
                else break;
            }

            // Check right side (>)
            for (int i = 0; i < DefaultDispersionRate; i++)
            {
                if (Context.IsEmpty(new(Context.Position.X + (i + 1), Context.Position.Y)))
                    rDistance++;
                else break;
            }

            // Check for the largest
            if ((int)MathF.Max(lDistance, rDistance) == lDistance)
            {
                targetDirection = -1;
                targetDistance = lDistance;
            }
            else
            {
                targetDirection = 1;
                targetDistance = rDistance;
            }

            // Set current position
            Context.TrySetPosition(new((Context.Position.X + targetDistance) * targetDirection, Context.Position.Y));
        }
    }
}