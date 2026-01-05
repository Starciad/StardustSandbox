using SkiaSharp;

using System;

namespace StardustSandbox.Extensions
{
    internal static class SKColorExtensions
    {
        internal static SKColor Darken(this SKColor color, float factor)
        {
            if (factor <= 0f)
            {
                return color;
            }

            byte r = (byte)Math.Max(0, (int)(color.Red * (1f - factor)));
            byte g = (byte)Math.Max(0, (int)(color.Green * (1f - factor)));
            byte b = (byte)Math.Max(0, (int)(color.Blue * (1f - factor)));
            return new SKColor(r, g, b, color.Alpha);
        }
    }
}
