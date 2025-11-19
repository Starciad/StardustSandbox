using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Interfaces;

using System;

namespace StardustSandbox.Managers
{
    internal sealed class CameraManager : IResettable
    {
        internal Vector2 Position { get; set; }
        internal float Rotation { get; set; }
        internal Vector2 Origin { get; set; }
        internal float Zoom
        {
            get => this.zoom;
            set
            {
                if (value < this.MinimumZoom || value > this.MaximumZoom)
                {
                    throw new ArgumentException("Zoom must be between MinimumZoom and MaximumZoom");
                }

                this.zoom = value;
            }
        }
        internal float MinimumZoom
        {
            get => this.minimumZoom;
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

                this.minimumZoom = value;
            }
        }
        internal float MaximumZoom
        {
            get => this.maximumZoom;
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

                this.maximumZoom = value;
            }
        }

        private float maximumZoom = float.MaxValue;
        private float minimumZoom;
        private float zoom;

        private readonly VideoManager videoManager;

        public void Reset()
        {
            this.Rotation = 0;
            this.Zoom = 1;
            this.Origin = new Vector2(this.videoManager.Viewport.Width, this.videoManager.Viewport.Height) / 2f;
            this.Position = Vector2.Zero;
        }

        internal CameraManager(VideoManager videoManager)
        {
            this.videoManager = videoManager;
            Reset();
        }

        internal void Move(Vector2 direction)
        {
            this.Position += Vector2.Transform(direction, Matrix.CreateRotationZ(-this.Rotation));
        }

        internal void Rotate(float deltaRadians)
        {
            this.Rotation += deltaRadians;
        }

        internal void ZoomIn(float deltaZoom)
        {
            ClampZoom(this.Zoom + deltaZoom);
        }

        internal void ZoomOut(float deltaZoom)
        {
            ClampZoom(this.Zoom - deltaZoom);
        }

        internal void ClampZoom(float value)
        {
            this.Zoom = value < this.MinimumZoom ? this.MinimumZoom : value > this.MaximumZoom ? this.MaximumZoom : value;
        }

        internal Vector2 WorldToScreen(Vector2 worldPosition)
        {
            Viewport viewport = this.videoManager.Viewport;
            return Vector2.Transform(worldPosition + new Vector2(viewport.X, viewport.Y), GetViewMatrix());
        }

        internal Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            Viewport viewport = this.videoManager.Viewport;
            return Vector2.Transform(screenPosition - new Vector2(viewport.X, viewport.Y), Matrix.Invert(GetViewMatrix()));
        }

        internal Matrix GetViewMatrix()
        {
            return GetVirtualViewMatrix() * Matrix.Identity;
        }

        private Matrix GetVirtualViewMatrix()
        {
            return Matrix.CreateTranslation(new(-this.Position.X, this.Position.Y, 0.0f)) *
                   Matrix.CreateTranslation(new(-this.Origin, 0.0f)) *
                   Matrix.CreateRotationZ(this.Rotation) *
                   Matrix.CreateScale(this.Zoom, this.Zoom, 1) *
                   Matrix.CreateTranslation(new(this.Origin, 0.0f));
        }

        internal bool InsideCameraBounds(Vector2 targetPosition, Point targetSize, bool inWorldPosition, float toleranceFactor = 0.0f)
        {
            Vector2 topLeft = targetPosition;
            Vector2 bottomRight = targetPosition + new Vector2(targetSize.X, targetSize.Y);

            topLeft -= new Vector2(toleranceFactor);
            bottomRight += new Vector2(toleranceFactor);

            Vector2 screenTopLeft = topLeft;
            Vector2 screenBottomRight = bottomRight;

            if (inWorldPosition)
            {
                screenTopLeft = WorldToScreen(topLeft);
                screenBottomRight = WorldToScreen(bottomRight);
            }

            // =========================== //

            // FAKE | DEBUG ONLY
            // Viewport temp = this._graphicsManager.Viewport;
            // Viewport viewport = new(temp.X, temp.Y, SScreenConstants.DEFAULT_SCREEN_WIDTH / 2, SScreenConstants.DEFAULT_SCREEN_HEIGHT / 2);

            // IN-GAME | FINAL
            Viewport viewport = this.videoManager.Viewport;

            // =========================== //

            return screenBottomRight.X >= 0 && screenTopLeft.X < viewport.Width &&
                   screenBottomRight.Y >= 0 && screenTopLeft.Y < viewport.Height;
        }
    }
}
