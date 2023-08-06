using Microsoft.Xna.Framework;

using PixelDust.Core.Utilities;

namespace PixelDust.Core.Extensions
{
    internal static class IColorExtensions
    {
        private static int VariationFactor => 8;

        public static Color Vary(this Color value)
        {
            int r, g, b;
            r = value.R + PRandom.Range(-VariationFactor, VariationFactor + 1);
            g = value.G + PRandom.Range(-VariationFactor, VariationFactor + 1);
            b = value.B + PRandom.Range(-VariationFactor, VariationFactor + 1);

            return new(r, g, b);
        }

        public static Color Darken(this Color value, uint amount)
        {
            return new(value.R - amount, value.G - amount, value.B - amount);
        }

        public static Color Clarify(this Color value, uint amount)
        {
            return new(value.R + amount, value.G + amount, value.B + amount);
        }
    }
}
