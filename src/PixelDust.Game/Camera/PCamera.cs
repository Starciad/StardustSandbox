using Microsoft.Xna.Framework;

namespace PixelDust.Game.Camera
{
    public abstract class PCamera
    {
        public abstract Vector2 Position { get; set; }
        public abstract float Rotation { get; set; }
        public abstract float Zoom { get; set; }
        public abstract float MinimumZoom { get; set; }
        public abstract float MaximumZoom { get; set; }
        public abstract Rectangle BoundingRectangle { get; }
        public abstract Vector2 Origin { get; set; }
        public abstract Vector2 Center { get; }

        public abstract void Move(Vector2 direction);
        public abstract void Rotate(float deltaRadians);
        public abstract void ZoomIn(float deltaZoom);
        public abstract void ZoomOut(float deltaZoom);
        public abstract void LookAt(Vector2 position);

        public abstract Vector2 WorldToScreen(Vector2 worldPosition);
        public abstract Vector2 ScreenToWorld(Vector2 screenPosition);

        public abstract Matrix GetViewMatrix();
        public abstract Matrix GetInverseViewMatrix();

        public abstract BoundingFrustum GetBoundingFrustum();
        public abstract ContainmentType Contains(Vector2 vector2);
        public abstract ContainmentType Contains(Rectangle rectangle);
    }
}
