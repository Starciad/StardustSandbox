using Microsoft.Xna.Framework;

using System;

namespace StardustSandbox.Core.Colors
{
    public sealed class SGradientColorMap
    {
        public (Color Start, Color End) Color1 { get; set; }
        public (Color Start, Color End) Color2 { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public float GetInterpolationFactor(TimeSpan currentTime)
        {
            double totalSeconds = (this.EndTime - this.StartTime).TotalSeconds;
            double elapsedSeconds = (currentTime - this.StartTime).TotalSeconds;

            return MathHelper.Clamp((float)(elapsedSeconds / totalSeconds), 0, 1);
        }
    }
}
