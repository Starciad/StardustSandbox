using System;

namespace StardustSandbox.Mathematics
{
    internal static class PercentageMath
    {
        internal static float PercentageOfValue(float value, float percentage)
        {
            return percentage < 0 || percentage > 100
                ? throw new ArgumentOutOfRangeException(nameof(percentage), "Percentage must be between 1 and 100.")
                : value * percentage / 100f;
        }

        internal static float PercentageFromValue(float total, float partial)
        {
            return total == 0 ? throw new ArgumentException("Total value cannot be zero.", nameof(total)) : partial / total * 100f;
        }
    }
}
