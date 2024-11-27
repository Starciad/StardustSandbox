using Microsoft.Xna.Framework;

using System;

namespace StardustSandbox.Core.Extensions
{
    public static class SPointExtensions
    {
        public static float Distance(this Point value1, Point value2)
        {
            float dx = value1.X - value2.X;
            float dy = value1.Y - value2.Y;

            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
