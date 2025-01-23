using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.Elements.Contexts;
using StardustSandbox.Core.Mathematics;

using System.Collections.Generic;

namespace StardustSandbox.Core.Elements.Utilities
{
    public static class SElementUtility
    {
        public static IEnumerable<Point> GetRandomSidePositions(Point targetPosition, SDirection direction)
        {
            int rDirection = SRandomMath.Chance(50, 100) ? 1 : -1;

            switch (direction)
            {
                case SDirection.Up:
                    yield return new(targetPosition.X, targetPosition.Y - 1);
                    yield return new(targetPosition.X + rDirection, targetPosition.Y - 1);
                    yield return new(targetPosition.X + (rDirection * -1), targetPosition.Y - 1);
                    break;

                case SDirection.Left:
                    yield return new(targetPosition.X + 1, targetPosition.Y);
                    yield return new(targetPosition.X + 1, targetPosition.Y + rDirection);
                    yield return new(targetPosition.X + 1, targetPosition.Y + (rDirection * -1));
                    break;

                case SDirection.Down:
                    yield return new(targetPosition.X, targetPosition.Y + 1);
                    yield return new(targetPosition.X + rDirection, targetPosition.Y + 1);
                    yield return new(targetPosition.X + (rDirection * -1), targetPosition.Y + 1);
                    break;

                case SDirection.Right:
                    yield return new(targetPosition.X - 1, targetPosition.Y);
                    yield return new(targetPosition.X - 1, targetPosition.Y + rDirection);
                    yield return new(targetPosition.X - 1, targetPosition.Y + (rDirection * -1));
                    break;

                default:
                    yield return new(targetPosition.X, targetPosition.Y + 1);
                    yield return new(targetPosition.X + rDirection, targetPosition.Y + 1);
                    yield return new(targetPosition.X + (rDirection * -1), targetPosition.Y + 1);
                    break;
            }
        }

        public static void NotifyFreeFallingFromAdjacentNeighbors(ISElementContext context, Point position)
        {
            context.SetElementFreeFalling(new(position.X - 1, position.Y), context.Layer, true);
            context.SetElementFreeFalling(new(position.X + 1, position.Y), context.Layer, true);
        }
    }
}
