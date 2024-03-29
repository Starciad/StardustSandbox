﻿using System;

namespace PixelDust.Game.Utilities
{
    public static class PRandom
    {
        private static readonly Random _random = new();

        public static int Range(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}
