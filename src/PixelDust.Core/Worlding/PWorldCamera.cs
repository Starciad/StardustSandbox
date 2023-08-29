using Microsoft.Xna.Framework;

namespace PixelDust.Core.Worlding
{
    public sealed class PWorldCamera
    {
        public Vector2 Position { get => position; set => position = value; }
        public float Rotation { get => rotation; set => rotation = value; }
        public float Zoom { get => zoom; set => zoom = value; }

        private Vector2 position = Vector2.Zero;
        private float rotation = 0f;
        private float zoom = 1f;

        public Matrix GetMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-position.X, position.Y, 0)) *
                   Matrix.CreateRotationZ(rotation) *
                   Matrix.CreateScale(zoom, zoom, 1.0f);
        }
    }
}
