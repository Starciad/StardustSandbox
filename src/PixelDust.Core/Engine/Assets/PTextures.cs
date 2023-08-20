using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PixelDust.Core.Engine
{
    public static class PTextures
    {
        internal static Texture2D Pixel { get; private set; }

        internal static void Load()
        {
            Pixel = new(PGraphics.GraphicsDevice, 1, 1);
            Pixel.SetData(new Color[] { Color.White });
        }

        internal static void Unload()
        {
            Pixel.Dispose();
        }
    }
}