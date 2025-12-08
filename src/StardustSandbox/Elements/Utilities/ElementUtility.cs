using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Randomness;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Utilities
{
    internal static class ElementUtility
    {
        internal static IEnumerable<Point> GetRandomSidePositions(Point targetPosition, Direction direction)
        {
            int rDirection = SSRandom.GetBool() ? 1 : -1;

            switch (direction)
            {
                case Direction.Up:
                    yield return new(targetPosition.X, targetPosition.Y - 1);
                    yield return new(targetPosition.X + rDirection, targetPosition.Y - 1);
                    yield return new(targetPosition.X + (rDirection * -1), targetPosition.Y - 1);
                    break;

                case Direction.Left:
                    yield return new(targetPosition.X + 1, targetPosition.Y);
                    yield return new(targetPosition.X + 1, targetPosition.Y + rDirection);
                    yield return new(targetPosition.X + 1, targetPosition.Y + (rDirection * -1));
                    break;

                case Direction.Down:
                    yield return new(targetPosition.X, targetPosition.Y + 1);
                    yield return new(targetPosition.X + rDirection, targetPosition.Y + 1);
                    yield return new(targetPosition.X + (rDirection * -1), targetPosition.Y + 1);
                    break;

                case Direction.Right:
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

        internal static void NotifyFreeFallingFromAdjacentNeighbors(in ElementContext context, Point position)
        {
            context.SetElementState(new(position.X - 1, position.Y), context.Layer, ElementStates.IsFalling);
            context.SetElementState(new(position.X + 1, position.Y), context.Layer, ElementStates.IsFalling);
        }
    }
}
