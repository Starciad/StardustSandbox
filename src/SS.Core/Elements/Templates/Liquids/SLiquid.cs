using Microsoft.Xna.Framework;

using StardustSandbox.Core.Elements.Utilities;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics;

namespace StardustSandbox.Core.Elements.Templates.Liquids
{
    public abstract class SLiquid(ISGame gameInstance) : SElement(gameInstance)
    {
        protected override void OnBehaviourStep()
        {
            Point[] belowPositions = SElementUtility.GetRandomSidePositions(this.Context.Position, SDirection.Down);

            if (this.Context.Slot.FreeFalling)
            {
                for (int i = 0; i < belowPositions.Length; i++)
                {
                    Point sidePos = belowPositions[i];
                    Point finalPos = new(sidePos.X, sidePos.Y);

                    if (this.Context.TrySetPosition(finalPos))
                    {
                        SElementUtility.NotifyFreeFallingFromAdjacentNeighbors(this.Context, finalPos);
                        this.Context.SetElementFreeFalling(finalPos, true);
                        return;
                    }
                }

                HorizontalMovementUpdate();
                this.Context.SetElementFreeFalling(false);
            }
            else
            {
                if (this.Context.TrySetPosition(new(this.Context.Position.X, this.Context.Position.Y + 1)))
                {
                    SElementUtility.NotifyFreeFallingFromAdjacentNeighbors(this.Context, belowPositions[0]);
                    this.Context.SetElementFreeFalling(belowPositions[0], true);
                    return;
                }
                else
                {
                    HorizontalMovementUpdate();
                    this.Context.SetElementFreeFalling(false);
                    return;
                }
            }
        }

        private void HorizontalMovementUpdate()
        {
            int rDirection = SRandomMath.Chance(50, 100) ? 1 : -1;
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
                switch (rDirection)
                {
                    case 1:
                        if (this.Context.TrySetPosition(rPosition))
                        { return; }

                        break;

                    case -1:
                        if (this.Context.TrySetPosition(lPosition))
                        { return; }

                        break;

                    default:
                        if (this.Context.TrySetPosition(rPosition))
                        { return; }

                        break;
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