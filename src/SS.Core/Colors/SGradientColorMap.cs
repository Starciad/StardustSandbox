using Microsoft.Xna.Framework;

using System;

namespace StardustSandbox.Core.Colors
{
    public sealed class SGradientColorMap
    {
        public Color InitialColor { get; set; }
        public Color FinalColor { get; set; }
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
