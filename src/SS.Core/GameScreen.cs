using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardustSandbox.Core
{
    internal static class GameScreen
    {
        private static GraphicsDevice graphicsDevice;

        internal static void Initialize(GraphicsDevice graphicsDevice)
        {
            GameScreen.graphicsDevice = graphicsDevice;
        }

        internal static Vector2 GetViewport()
        {
            return new(
                graphicsDevice.Viewport.Width,
                graphicsDevice.Viewport.Height
            );
        }

        internal static Vector2 GetViewportCenter()
        {
            return GetViewport() / 2.0f;
        }
    }
}
