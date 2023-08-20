using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PixelDust.Core.Engine
{
    /// <summary>
    /// Static class responsible for storing all the most relevant information about the game's graphics settings.
    /// </summary>
    public static class PGraphics
    {
        public static GraphicsDeviceManager GraphicsDeviceManager => _graphicsDeviceManager;
        public static GraphicsDevice GraphicsDevice => _graphicsDeviceManager.GraphicsDevice;
        public static SpriteBatch SpriteBatch => _spriteBatch;

        internal static Viewport Viewport => GraphicsDevice.Viewport;
        internal static RenderTarget2D DefaultRenderTarget => _renderTarget;

        private static GraphicsDeviceManager _graphicsDeviceManager;
        private static SpriteBatch _spriteBatch;
        private static RenderTarget2D _renderTarget;

        internal static void Build(GraphicsDeviceManager graphicsDeviceManager)
        {
            _graphicsDeviceManager = graphicsDeviceManager;
        }

        internal static void Load()
        {
            _spriteBatch = new(GraphicsDevice);
            _renderTarget = new(GraphicsDevice, (int)PScreen.DefaultResolution.X, (int)PScreen.DefaultResolution.Y, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
        }
    }
}
