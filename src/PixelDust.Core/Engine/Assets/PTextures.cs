using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PixelDust.Core.Engine
{
    /// <summary>
    /// Static class responsible for managing, storing and configuring all textures in the game.
    /// </summary>
    public static class PTextures
    {
        public static Texture2D Pixel { get; private set; }
        public static Texture2D Elements { get; private set; }

        /// <summary>
        /// Loads all base textures that will be used in the project.
        /// </summary>
        internal static void Load()
        {
            Pixel = new(PGraphics.GraphicsDevice, 1, 1);
            Pixel.SetData(new Color[] { Color.White });

            Elements = PContent.Sprites.Load<Texture2D>("Elements");
        }

        /// <summary>
        /// Unloads all instanced textures.
        /// </summary>
        internal static void Unload()
        {
            Pixel.Dispose();
        }
    }
}