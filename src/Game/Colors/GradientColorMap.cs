using Microsoft.Xna.Framework;

using System;

namespace StardustSandbox.Colors
{
    internal sealed class GradientColorMap
    {
        internal GradientColor GradientStartColor { get; init; }
        internal GradientColor GradientEndColor { get; init; }
        internal TimeSpan StartTime { get; init; }
        internal TimeSpan EndTime { get; init; }

        internal float GetInterpolationFactor(TimeSpan currentTime)
        {
            float totalSeconds = Convert.ToSingle((this.EndTime - this.StartTime).TotalSeconds);
            float elapsedSeconds = Convert.ToSingle((currentTime - this.StartTime).TotalSeconds);

            return MathHelper.Clamp(Convert.ToSingle(elapsedSeconds / totalSeconds), 0, 1);
        }
    }
}
