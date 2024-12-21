using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Mathematics;

namespace StardustSandbox.Core.Elements.Utilities
{
    public static class SElementUtility
    {
        public static Point[] GetRandomSidePositions(Point targetPosition, SDirection direction)
        {
            int rDirection = SRandomMath.Chance(50, 100) ? 1 : -1;

            return direction switch
            {
                SDirection.Up => [
                    new(targetPosition.X, targetPosition.Y - 1),
                    new(targetPosition.X + rDirection, targetPosition.Y - 1),
                    new(targetPosition.X + (rDirection * -1), targetPosition.Y - 1),
                ],

                SDirection.Left => [
                    new(targetPosition.X + 1, targetPosition.Y),
                    new(targetPosition.X + 1, targetPosition.Y + rDirection),
                    new(targetPosition.X + 1, targetPosition.Y + (rDirection * -1)),
                ],

                SDirection.Down => [
                    new(targetPosition.X, targetPosition.Y + 1),
                    new(targetPosition.X + rDirection, targetPosition.Y + 1),
                    new(targetPosition.X + (rDirection * -1), targetPosition.Y + 1),
                ],

                SDirection.Right => [
                    new(targetPosition.X - 1, targetPosition.Y),
                    new(targetPosition.X - 1, targetPosition.Y + rDirection),
                    new(targetPosition.X - 1, targetPosition.Y + (rDirection * -1)),
                ],

                _ => [
                    new(targetPosition.X, targetPosition.Y + 1),
                    new(targetPosition.X + rDirection, targetPosition.Y + 1),
                    new(targetPosition.X + (rDirection * -1), targetPosition.Y + 1),
                ],
            };
        }

        public static void NotifyFreeFallingFromAdjacentNeighbors(ISElementContext context, Point position)
        {
            context.SetElementFreeFalling(context.Layer, new(position.X - 1, position.Y), true);
            context.SetElementFreeFalling(context.Layer, new(position.X + 1, position.Y), true);
        }

        public static void UpdateHorizontalPosition(ISElementContext context, int dispersionRate)
        {
            Point currentPosition = context.Slot.Position;

            (Point leftPos, Point rightPos) = GetSidewaysSpreadPositions(context, currentPosition, dispersionRate);

            float leftDistance = SPointExtensions.Distance(currentPosition, leftPos);
            float rightDistance = SPointExtensions.Distance(currentPosition, rightPos);

            Point targetPosition = leftDistance == rightDistance
                ? (SRandomMath.Chance(50, 101) ? leftPos : rightPos)
                : (leftDistance > rightDistance ? leftPos : rightPos);

            _ = context.TrySetPosition(context.Layer, targetPosition);
        }

        public static (Point left, Point right) GetSidewaysSpreadPositions(ISElementContext context, Point position, int rate)
        {
            return (
                GetDispersionPosition(context, position, rate, -1),
                GetDispersionPosition(context, position, rate, 1)
            );
        }

        private static Point GetDispersionPosition(ISElementContext context, Point position, int rate, int direction)
        {
            Point dispersionPosition = position;

            for (int i = 0; i < rate; i++)
            {
                Point nextPosition = new(dispersionPosition.X + direction, dispersionPosition.Y);

                if (context.IsEmptyElementSlot(nextPosition))
                {
                    dispersionPosition = nextPosition;
                }
                else
                {
                    break;
                }
            }

            return dispersionPosition;
        }
    }
}
