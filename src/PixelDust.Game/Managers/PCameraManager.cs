using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Constants;
using PixelDust.Game.Mathematics;

using System;

namespace PixelDust.Game.Managers
{
    public sealed class PCameraManager
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Center => this.Position + this.Origin;
        public float Zoom
        {
            get => this._zoom;
            set
            {
                if (value < this.MinimumZoom || value > this.MaximumZoom)
                {
                    throw new ArgumentException("Zoom must be between MinimumZoom and MaximumZoom");
                }

                this._zoom = value;
            }
        }
        public float MinimumZoom
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
        public float MaximumZoom
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

        private float _maximumZoom = float.MaxValue;
        private float _minimumZoom;
        private float _zoom;

        private readonly PGraphicsManager _graphicsManager;

        public PCameraManager(PGraphicsManager graphicsManager)
        {
            this._graphicsManager = graphicsManager;

            this.Rotation = 0;
            this.Zoom = 1;
            this.Origin = new Vector2(this._graphicsManager.Viewport.Width, this._graphicsManager.Viewport.Height) / 2f;
            this.Position = Vector2.Zero;
        }

        public void Move(Vector2 direction)
        {
            this.Position += Vector2.Transform(direction, Matrix.CreateRotationZ(-this.Rotation));
        }

        public void Rotate(float deltaRadians)
        {
            this.Rotation += deltaRadians;
        }

        public void ZoomIn(float deltaZoom)
        {
            ClampZoom(this.Zoom + deltaZoom);
        }

        public void ZoomOut(float deltaZoom)
        {
            ClampZoom(this.Zoom - deltaZoom);
        }

        private void ClampZoom(float value)
        {
            this.Zoom = value < this.MinimumZoom ? this.MinimumZoom : value > this.MaximumZoom ? this.MaximumZoom : value;
        }

        public void LookAt(Vector2 position)
        {
            this.Position = position - new Vector2(PScreenConstants.DEFAULT_SCREEN_WIDTH, PScreenConstants.DEFAULT_SCREEN_HEIGHT) / 2f;
        }

        public Vector2 WorldToScreen(float x, float y)
        {
            return WorldToScreen(new Vector2(x, y));
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            Viewport viewport = this._graphicsManager.Viewport;
            return Vector2.Transform(worldPosition + new Vector2(viewport.X, viewport.Y), GetViewMatrix());
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            Viewport viewport = this._graphicsManager.Viewport;
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

        public Matrix GetViewMatrix()
        {
            return GetVirtualViewMatrix() * Matrix.Identity;
        }

        public bool InsideCameraBounds(Vector2 targetPosition, Size2 targetSize, float toleranceFactor = 0f)
        {
            Vector2 topLeft = targetPosition;
            Vector2 bottomRight = targetPosition + new Vector2(targetSize.Width, targetSize.Height);

            topLeft -= new Vector2(toleranceFactor);
            bottomRight += new Vector2(toleranceFactor);

            Vector2 screenTopLeft = WorldToScreen(topLeft);
            Vector2 screenBottomRight = WorldToScreen(bottomRight);

            Viewport viewport = this._graphicsManager.Viewport;

            return screenBottomRight.X > 0 && screenTopLeft.X < viewport.Width &&
                   screenBottomRight.Y > 0 && screenTopLeft.Y < viewport.Height;
        }
    }
}
