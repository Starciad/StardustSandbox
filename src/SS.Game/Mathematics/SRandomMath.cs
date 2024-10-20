using System;

namespace StardustSandbox.Game.Mathematics
{
    public static class SRandomMath
    {
        private static readonly Random _random = new();

        public static double RandomDouble()
        {
            return _random.NextDouble();
        }

        public static int Range(int min, int max)
        {
            return _random.Next(min, max);
        }

        public static bool Chance(int chance, int total)
        {
            return Range(0, total) < chance;
        }
    }
}
