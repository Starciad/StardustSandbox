﻿using Microsoft.Xna.Framework;

using StardustSandbox.Game.General;

namespace StardustSandbox.Game.Elements.Common.Liquid
{
    public abstract class SLiquid : SElement
    {
        public override void OnBehaviourStep()
        {
            int direction = SRandom.Range(0, 101) < 50 ? 1 : -1;

            Point down = new(this.Context.Position.X, this.Context.Position.Y + 1);
            Point[] sides =
            [
                new Point(this.Context.Position.X + direction, this.Context.Position.Y),
                new Point(this.Context.Position.X + (direction * -1), this.Context.Position.Y),
            ];

            if (this.Context.TrySetPosition(down))
            {
                return;
            }

            foreach (Point side in sides)
            {
                if (this.Context.TrySetPosition(new(side.X, side.Y + 1)))
                { return; }
            }

            HorizontalMovementUpdate(direction);
        }

        private void HorizontalMovementUpdate(int direction)
        {
            int lCount = 0, rCount = 0;
            bool lCountBreak = false, rCountBreak = false;

            // Left (<) && Right (>)
            for (int i = 0; i < this.DefaultDispersionRate; i++)
            {
                if (lCountBreak && rCountBreak)
                {
                    break;
                }

                if (!lCountBreak && this.Context.IsEmptyElementSlot(new(this.Context.Position.X - (i + 1), this.Context.Position.Y)))
                {
                    lCount++;
                }
                else
                {
                    lCountBreak = true;
                }

                if (!rCountBreak && this.Context.IsEmptyElementSlot(new(this.Context.Position.X + i + 1, this.Context.Position.Y)))
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

            Point lPosition = new(this.Context.Position.X - tCount, this.Context.Position.Y);
            Point rPosition = new(this.Context.Position.X + tCount, this.Context.Position.Y);

            if (tCount.Equals(lCount) && tCount.Equals(rCount))
            {
                if (direction.Equals(-1))
                {
                    if (this.Context.TrySetPosition(lPosition))
                    { return; }
                }

                if (direction.Equals(1))
                {
                    if (this.Context.TrySetPosition(rPosition))
                    { return; }
                }
            }

            if (tCount.Equals(lCount))
            {
                if (this.Context.TrySetPosition(lPosition))
                { return; }
            }

            if (tCount.Equals(rCount))
            {
                if (this.Context.TrySetPosition(rPosition))
                { return; }
            }
        }
    }
}