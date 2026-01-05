using Microsoft.Xna.Framework;

using SkiaSharp;

using System;

namespace StardustSandbox.Extensions
{
    internal static class PointExtensions
    {
        internal static float Distance(this Point value1, Point value2)
        {
            float dx = value1.X - value2.X;
            float dy = value1.Y - value2.Y;

            return MathF.Sqrt((dx * dx) + (dy * dy));
        }

        internal static SKPointI ToSKPointI(this Point point)
        {
            return new(point.X, point.Y);
        }
    }
}
