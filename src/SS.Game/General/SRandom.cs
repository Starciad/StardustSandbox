using System;

namespace StardustSandbox.Game.General
{
    public static class SRandom
    {
        private static readonly Random _random = new();

        public static int Range(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}
