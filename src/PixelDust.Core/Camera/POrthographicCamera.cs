using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Engine.Components;

using System;

namespace PixelDust.Core.Camera
{
    public sealed class POrthographicCamera : PCamera
    {
        private float _maximumZoom = float.MaxValue;
        private float _minimumZoom;
        private float _zoom;

        public POrthographicCamera()
        {
            this.Rotation = 0;
            this.Zoom = 1;
            this.Origin = new Vector2(PScreen.DefaultResolution.X / 2f, PScreen.DefaultResolution.Y / 2f);
            this.Position = Vector2.Zero;
        }

        public override Vector2 Position { get; set; }
        public override float Rotation { get; set; }
        public override Vector2 Origin { get; set; }
        public override Vector2 Center => this.Position + this.Origin;

        public override float Zoom
        {
            get => this._zoom;
            set
            {
                if ((value < this.MinimumZoom) || (value > this.MaximumZoom))
                {
                    throw new ArgumentException("Zoom must be between MinimumZoom and MaximumZoom");
                }

                this._zoom = value;
            }
        }

        public override float MinimumZoom
        {
            get => this._minimumZoom;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("MinimumZoom must be greater than zero");
                }

                if (this.Zoom < value)
                {
                    this.Zoom = this.MinimumZoom;
                }

                this._minimumZoom = value;
            }
        }

        public override float MaximumZoom
        {
            get => this._maximumZoom;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("MaximumZoom must be greater than zero");
                }

                if (this.Zoom > value)
                {
                    this.Zoom = value;
                }

                this._maximumZoom = value;
            }
        }

        public override Rectangle BoundingRectangle
        {
            get
            {
                BoundingFrustum frustum = GetBoundingFrustum();
                Vector3[] corners = frustum.GetCorners();
                Vector3 topLeft = corners[0];
                Vector3 bottomRight = corners[2];
                int width = (int)(bottomRight.X - topLeft.X);
                int height = (int)(bottomRight.Y - topLeft.Y);

                return new(new((int)topLeft.X, (int)topLeft.Y), new(width, height));
            }
        }

        public override void Move(Vector2 direction)
        {
            this.Position += Vector2.Transform(direction, Matrix.CreateRotationZ(-this.Rotation));
        }

        public override void Rotate(float deltaRadians)
        {
            this.Rotation += deltaRadians;
        }

        public override void ZoomIn(float deltaZoom)
        {
            ClampZoom(this.Zoom + deltaZoom);
        }

        public override void ZoomOut(float deltaZoom)
        {
            ClampZoom(this.Zoom - deltaZoom);
        }

        private void ClampZoom(float value)
        {
            this.Zoom = value < this.MinimumZoom ? this.MinimumZoom : value > this.MaximumZoom ? this.MaximumZoom : value;
        }

        public override void LookAt(Vector2 position)
        {
            this.Position = position - new Vector2(PScreen.DefaultResolution.X / 2f, PScreen.DefaultResolution.Y / 2f);
        }

        public Vector2 WorldToScreen(float x, float y)
        {
            return WorldToScreen(new Vector2(x, y));
        }

        public override Vector2 WorldToScreen(Vector2 worldPosition)
        {
            Viewport viewport = PScreen.Viewport;
            return Vector2.Transform(worldPosition + new Vector2(viewport.X, viewport.Y), GetViewMatrix());
        }

        public override Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            Viewport viewport = PScreen.Viewport;
            return Vector2.Transform(screenPosition - new Vector2(viewport.X, viewport.Y),
                   Matrix.Invert(GetViewMatrix()));
        }

        private Matrix GetVirtualViewMatrix()
        {
            return Matrix.CreateTranslation(new(-this.Position.X, this.Position.Y, 0.0f)) *
                   Matrix.CreateTranslation(new(-this.Origin, 0.0f)) *
                   Matrix.CreateRotationZ(this.Rotation) *
                   Matrix.CreateScale(this.Zoom, this.Zoom, 1) *
                   Matrix.CreateTranslation(new(this.Origin, 0.0f));
        }

        public override Matrix GetViewMatrix()
        {
            return GetVirtualViewMatrix() * Matrix.Identity;
        }

        public override Matrix GetInverseViewMatrix()
        {
            return Matrix.Invert(GetViewMatrix());
        }

        private static Matrix GetProjectionMatrix(Matrix viewMatrix)
        {
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, PScreen.DefaultResolution.X, PScreen.DefaultResolution.Y, 0, -1, 0);
            Matrix.Multiply(ref viewMatrix, ref projection, out projection);
            return projection;
        }

        public override BoundingFrustum GetBoundingFrustum()
        {
            Matrix viewMatrix = GetVirtualViewMatrix();
            Matrix projectionMatrix = GetProjectionMatrix(viewMatrix);
            return new(projectionMatrix);
        }

        public ContainmentType Contains(Point point)
        {
            return Contains(point.ToVector2());
        }

        public override ContainmentType Contains(Vector2 vector2)
        {
            return GetBoundingFrustum().Contains(new Vector3(vector2.X, vector2.Y, 0));
        }

        public override ContainmentType Contains(Rectangle rectangle)
        {
            Vector3 max = new(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height, 0.5f);
            Vector3 min = new(rectangle.X, rectangle.Y, 0.5f);
            BoundingBox boundingBox = new(min, max);
            return GetBoundingFrustum().Contains(boundingBox);
        }
    }
}
