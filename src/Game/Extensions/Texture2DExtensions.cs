using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SkiaSharp;

namespace StardustSandbox.Extensions
{
    internal static class Texture2DExtensions
    {
        internal static Point GetSize(this Texture2D texture)
        {
            return new(texture.Width, texture.Height);
        }

        internal static SKImage ToSKImage(this Texture2D texture)
        {
            if (texture == null)
            {
                return null;
            }

            int width = texture.Width;
            int height = texture.Height;

            // Create array to store pixel data
            int[] pixelData = new int[width * height];

            // Get pixel data from Texture2D
            texture.GetData(pixelData);

            // Create SKBitmap and set its pixels
            SKBitmap bitmap = new(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);

            unsafe
            {
                fixed (int* ptr = pixelData)
                {
                    _ = bitmap.InstallPixels(
                        new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Premul),
                        new nint(ptr),
                        width * sizeof(int)
                    );
                }
            }

            // Create SKImage from SKBitmap
            SKImage skImage = SKImage.FromBitmap(bitmap);

            // We can safely dispose bitmap after creating the image
            bitmap.Dispose();

            return skImage;
        }
    }
}
