using System;

namespace StardustSandbox.Game.Mathematics
{
    public static class SPercentageMath
    {
        public static float PercentageOfValue(float value, float percentage)
        {
            return percentage < 0 || percentage > 100
                ? throw new ArgumentOutOfRangeException(nameof(percentage), "Percentage must be between 1 and 100.")
                : value * percentage / 100f;
        }

        public static float PercentageFromValue(float total, float partial)
        {
            return total == 0 ? throw new ArgumentException("Total value cannot be zero.", nameof(total)) : partial / total * 100f;
        }
    }
}
