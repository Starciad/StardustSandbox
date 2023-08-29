using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelDust.Core.Engine
{
    internal static class PCamera
    {
        public static Vector2 Postion { get => position; set => position = value; }
        public static float Rotation { get => rotation; set => rotation = value; }
        public static float Zoom { get => zoom; set => zoom = value; }

        private static Vector2 position = Vector2.Zero;
        private static float rotation = 0f;
        private static float zoom = 1f;
        private static Viewport viewport;

        internal static void Initialize()
        {
            viewport = PGraphics.GraphicsDevice.Viewport;
        }

        public static Matrix GetMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-position.X, position.Y, 0)) *
                   Matrix.CreateRotationZ(rotation) *
                   Matrix.CreateScale(zoom, zoom, 1.0f);
        }
    }
}
