using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.General;
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
            context.SetElementFreeFalling(new(position.X - 1, position.Y), true);
            context.SetElementFreeFalling(new(position.X + 1, position.Y), true);
        }
    }
}
