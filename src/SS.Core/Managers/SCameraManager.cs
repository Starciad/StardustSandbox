using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics.Primitives;

using System;

namespace StardustSandbox.Core.Managers
{
    public sealed class SCameraManager : SManager
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
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

        private readonly SGraphicsManager _graphicsManager;

        public SCameraManager(ISGame gameInstance) : base(gameInstance)
        {
            this._graphicsManager = gameInstance.GraphicsManager;

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

        public void ClampZoom(float value)
        {
            this.Zoom = value < this.MinimumZoom ? this.MinimumZoom : value > this.MaximumZoom ? this.MaximumZoom : value;
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            Viewport viewport = this._graphicsManager.Viewport;
            return Vector2.Transform(worldPosition + new Vector2(viewport.X, viewport.Y), GetViewMatrix());
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            Viewport viewport = this._graphicsManager.Viewport;
            return Vector2.Transform(screenPosition - new Vector2(viewport.X, viewport.Y), Matrix.Invert(GetViewMatrix()));
        }

        public Matrix GetViewMatrix()
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

        public bool InsideCameraBounds(Vector2 targetPosition, SSize2 targetSize, bool inWorldPosition, float toleranceFactor = 0f)
        {
            Vector2 topLeft = targetPosition;
            Vector2 bottomRight = targetPosition + new Vector2(targetSize.Width, targetSize.Height);

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
            Viewport viewport = this._graphicsManager.Viewport;

            // =========================== //

            return screenBottomRight.X >= 0 && screenTopLeft.X < viewport.Width &&
                   screenBottomRight.Y >= 0 && screenTopLeft.Y < viewport.Height;
        }
    }
}
