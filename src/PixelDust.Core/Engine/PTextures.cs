using Microsoft.Xna.Framework.Graphics;

namespace PixelDust.Core
{
    public static class PTextures
    {
        internal static Texture2D Pixel { get; private set; }

        internal static void Load()
        {
            Pixel = new(PGraphics.GraphicsDevice, 1, 1);
            Pixel.SetData(new Color[] { Color.White });
        }
    }
}
