using Microsoft.Xna.Framework;

using StardustSandbox.Core.Mathematics;

using System;

namespace StardustSandbox.Core.Extensions
{
    public static class SColorExtensions
    {
        public static Color Vary(this Color baseColor, int variationFactor, bool varyAlpha = false)
        {
            variationFactor = Math.Clamp(variationFactor, 0, 255);

            int VaryChannel(int channel)
            {
                int randomOffset = SRandomMath.Range(-variationFactor, variationFactor + 1);
                return Math.Clamp(channel + randomOffset, 0, 255);
            }

            int r = VaryChannel(baseColor.R);
            int g = VaryChannel(baseColor.G);
            int b = VaryChannel(baseColor.B);
            int a = baseColor.A;

            if (varyAlpha)
            {
                a = VaryChannel(baseColor.A);
            }
            

            return new Color(r, g, b, a);
        }
    }
}
