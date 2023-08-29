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
                new(PElementContext.Position.X                   , PElementContext.Position.Y + 1),
                new(PElementContext.Position.X + direction       , PElementContext.Position.Y + 1),
                new(PElementContext.Position.X + direction * -1  , PElementContext.Position.Y + 1),
            };

            // Down
            // Liquido tries to move to target positions
            foreach (Vector2 targetPos in targets)
            {
                if (PElementContext.IsEmpty(targetPos))
                    if (PElementContext.TrySetPosition(targetPos)) return;
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
                if (PElementContext.IsEmpty(new(PElementContext.Position.X - (i + 1), PElementContext.Position.Y)))
                    lDistance++;
                else break;
            }

            // Check right side (>)
            for (int i = 0; i < DefaultDispersionRate; i++)
            {
                if (PElementContext.IsEmpty(new(PElementContext.Position.X + (i + 1), PElementContext.Position.Y)))
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
            PElementContext.TrySetPosition(new((PElementContext.Position.X + targetDistance) * targetDirection, PElementContext.Position.Y));
        }
    }
}