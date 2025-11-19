using Microsoft.Xna.Framework;

using StardustSandbox.Randomness;

using System;

namespace StardustSandbox.Extensions
{
    internal static class ColorExtensions
    {
        internal static Color Vary(this Color baseColor, int variationFactor, bool varyAlpha = false)
        {
            variationFactor = Math.Clamp(variationFactor, 0, 255);

            int VaryChannel(int channel)
            {
                int randomOffset = SSRandom.Range(-variationFactor, variationFactor + 1);
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

            return new(r, g, b, a);
        }

        internal static Color Darken(this Color baseColor, float darkenFactor)
        {
            darkenFactor = Math.Clamp(darkenFactor, 0f, 1f);

            int r = (int)(baseColor.R * (1 - darkenFactor));
            int g = (int)(baseColor.G * (1 - darkenFactor));
            int b = (int)(baseColor.B * (1 - darkenFactor));

            return new(r, g, b, baseColor.A);
        }

        internal static Color OverlayBlend(this Color baseColor, Color overlayColor, float blendFactor)
        {
            blendFactor = Math.Clamp(blendFactor, 0f, 1f);

            int r = (int)(baseColor.R + ((overlayColor.R - baseColor.R) * blendFactor));
            int g = (int)(baseColor.G + ((overlayColor.G - baseColor.G) * blendFactor));
            int b = (int)(baseColor.B + ((overlayColor.B - baseColor.B) * blendFactor));
            int a = (int)(baseColor.A + ((overlayColor.A - baseColor.A) * blendFactor));

            return new(r, g, b, a);
        }
    }
}
