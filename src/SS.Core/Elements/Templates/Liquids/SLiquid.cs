using Microsoft.Xna.Framework;

using StardustSandbox.Core.Elements.Utilities;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics;

using System;

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
            Point currentPosition = this.Context.Position;

            (Point leftDispersionPosition, Point rightDispersionPosition) = GetSidewaysSpreadPositions(this.Context, currentPosition, this.DefaultDispersionRate);

            float leftDistance = SPointExtensions.Distance(currentPosition, leftDispersionPosition);
            float rightDistance = SPointExtensions.Distance(currentPosition, rightDispersionPosition);

            // When the distances are equal, the position decided will be random.
            if (leftDistance.Equals(rightDistance))
            {
                if (SRandomMath.Chance(50, 101))
                {
                    this.Context.TrySetPosition(leftDispersionPosition);
                }
                else
                {
                    this.Context.TrySetPosition(rightDispersionPosition);
                }
                                
                return;
            }

            // If the distances are not equal, the element chooses the largest one to position itself.
            if (leftDistance > rightDistance)
            {
                this.Context.TrySetPosition(leftDispersionPosition);
            }
            else
            {
                this.Context.TrySetPosition(rightDispersionPosition);
            }
        }

        private static (Point leftDispersionPosition, Point rightDispersionPosition) GetSidewaysSpreadPositions(ISElementContext elementContext, Point currentPosition, int dispersionRate)
        {
            Point leftDispersionPosition = currentPosition;
            Point rightDispersionPosition = currentPosition;

            bool leftIsObstructed = false;
            bool rightIsObstructed = false;

            for (int i = 0; i < dispersionRate; i++)
            {
                // Left (<)
                if (!leftIsObstructed)
                {
                    if (elementContext.IsEmptyElementSlot(new Point(leftDispersionPosition.X - 1, leftDispersionPosition.Y)))
                    {
                        leftDispersionPosition.X--;
                    }
                    else
                    {
                        leftIsObstructed = true;
                    }
                }

                // Right (>)
                if (!rightIsObstructed)
                {
                    if (elementContext.IsEmptyElementSlot(new Point(rightDispersionPosition.X + 1, rightDispersionPosition.Y)))
                    {
                        rightDispersionPosition.X++;
                    }
                    else
                    {
                        rightIsObstructed = true;
                    }
                }
            }

            return (leftDispersionPosition, rightDispersionPosition);
        }
    }
}