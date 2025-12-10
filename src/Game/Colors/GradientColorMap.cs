using Microsoft.Xna.Framework;

using System;

namespace StardustSandbox.Colors
{
    internal sealed class GradientColorMap
    {
        internal (Color Start, Color End) Color1 { get; set; }
        internal (Color Start, Color End) Color2 { get; set; }
        internal TimeSpan StartTime { get; set; }
        internal TimeSpan EndTime { get; set; }

        internal float GetInterpolationFactor(TimeSpan currentTime)
        {
            float totalSeconds = Convert.ToSingle((this.EndTime - this.StartTime).TotalSeconds);
            float elapsedSeconds = Convert.ToSingle((currentTime - this.StartTime).TotalSeconds);

            return MathHelper.Clamp(Convert.ToSingle(elapsedSeconds / totalSeconds), 0, 1);
        }
    }
}
