using Microsoft.Xna.Framework;

using PixelDust.Core.Utilities;

using System;
using System.Threading.Tasks;

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

            // Liquido tries to move to target positions
            foreach (Vector2 targetPos in targets)
            {
                if (Context.IsEmpty(targetPos))
                    if (Context.TrySetPosition(targetPos)) return;
            }

            LiquidDispersion();
        }

        private void LiquidDispersion()
        {
            // If the liquid does not find gaps above, it advances to the left or right based on the dispersion value in search of the closest gap.If it does not find available gaps next to it, it takes the longest path.
            int rightPathSearch = 0;
            int leftPathSearch = 0;
            bool unevennessFound = false;

            // Right search (>)
            for (int i = 0; i < DefaultDispersionRate; i++)
            {
                Vector2 targetPos = new(Context.Position.X + 1, Context.Position.Y);

                if (Context.IsEmpty(targetPos)) rightPathSearch++;
                if (Context.IsEmpty(new(targetPos.X, targetPos.Y + 1)))
                {
                    rightPathSearch++;
                    unevennessFound = true;
                    break;
                }
            }

            // Left search (<)
            for (int i = 0; i < DefaultDispersionRate; i++)
            {
                Vector2 targetPos = new(Context.Position.X - 1, Context.Position.Y);

                if (Context.IsEmpty(targetPos)) leftPathSearch++;
                if (Context.IsEmpty(new(targetPos.X, targetPos.Y + 1)))
                {
                    leftPathSearch++;
                    unevennessFound = true;
                    break;
                }
            }

            // Set new position
            Vector2 newPos;
            if (unevennessFound)
            {
                if (MathF.Max(rightPathSearch, leftPathSearch) == rightPathSearch) // Right
                {
                    newPos = new(Context.Position.X + rightPathSearch, Context.Position.Y);
                }
                else // Left
                {
                    newPos = new(Context.Position.X - leftPathSearch, Context.Position.Y);
                }
            }
            else
            {
                int dispersionValue = PRandom.Range(1, 101) < 50 ? -leftPathSearch : rightPathSearch;
                newPos = new(Context.Position.X + dispersionValue, Context.Position.Y);
            }
                
            Context.TrySetPosition(newPos);
        }
    }
}