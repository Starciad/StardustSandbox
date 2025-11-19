using Microsoft.Xna.Framework;

using System.Collections.Generic;

namespace StardustSandbox.Mathematics
{
    internal static class ShapePointGenerator
    {
        internal static IEnumerable<Point> GenerateCirclePoints(Point center, int radius)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    if ((x * x) + (y * y) <= radius * radius)
                    {
                        yield return new(center.X + x, center.Y + y);
                    }
                }
            }
        }

        internal static IEnumerable<Point> GenerateSquarePoints(Point center, int radius)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    yield return new(center.X + x, center.Y + y);
                }
            }
        }

        internal static IEnumerable<Point> GenerateTrianglePoints(Point center, int height)
        {
            for (int y = 0; y <= height; y++)
            {
                int rowWidth = height - y;
                for (int x = -rowWidth; x <= rowWidth; x++)
                {
                    yield return new(center.X + x, center.Y - y + (height / 2));
                }
            }
        }
    }
}
