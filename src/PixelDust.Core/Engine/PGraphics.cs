using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PixelDust.Core
{
    public static class PGraphics
    {
        public static GraphicsDeviceManager GraphicsDeviceManager => _graphicsDeviceManager;
        public static GraphicsDevice GraphicsDevice => _graphicsDeviceManager.GraphicsDevice;
        public static SpriteBatch SpriteBatch => _spriteBatch;
        public static RenderTarget2D RenderTarget => _renderTarget;
        public static Viewport Viewport => GraphicsDevice.Viewport;

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
            _renderTarget = new(GraphicsDevice, PScreen.DefaultWidth, PScreen.DefaultHeight);
        }
    }
}
