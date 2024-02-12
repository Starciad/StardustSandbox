using System;

namespace PixelDust.Game.Mathematics
{
    public static class PPercentageMath
    {
        public static float PercentageOfValue(float value, float percentage)
        {
            if (percentage < 0 || percentage > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(percentage), "Percentage must be between 1 and 100.");
            }

            return (value * percentage) / 100f;
        }

        public static float PercentageFromValue(float total, float partial)
        {
            if (total == 0)
            {
                throw new ArgumentException("Total value cannot be zero.", nameof(total));
            }

            return (partial / total) * 100f;
        }
    }
}
