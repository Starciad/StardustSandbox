using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Managers;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.Camera
{
    internal static class SSCamera
    {
        internal static Vector2 Position { get; set; }
        internal static float Rotation { get; set; }
        internal static Vector2 Origin { get; set; }
        internal static float Zoom
        {
            get => zoom;
            set
            {
                if (value < MinimumZoom || value > MaximumZoom)
                {
                    throw new ArgumentException("Zoom must be between MinimumZoom and MaximumZoom");
                }

                zoom = value;
            }
        }
        internal static float MinimumZoom
        {
            get => minimumZoom;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("MinimumZoom must be greater than zero");
                }

                if (Zoom < value)
                {
                    Zoom = MinimumZoom;
                }

                minimumZoom = value;
            }
        }
        internal static float MaximumZoom
        {
            get => maximumZoom;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("MaximumZoom must be greater than zero");
                }

                if (Zoom > value)
                {
                    Zoom = value;
                }

                maximumZoom = value;
            }
        }

        private static float maximumZoom = float.MaxValue;
        private static float minimumZoom;
        private static float zoom;

        private static bool isInitialized = false;

        private static VideoManager videoManager;

        internal static void Reset()
        {
            Rotation = 0;
            Zoom = 1;
            Origin = new Vector2(videoManager.Viewport.Width, videoManager.Viewport.Height) / 2f;
            Position = Vector2.Zero;
        }

        internal static void Initialize(VideoManager videoManager)
        {
            if (isInitialized)
            {
                throw new InvalidOperationException($"{nameof(SSCamera)} is already initialized");
            }

            SSCamera.videoManager = videoManager;
            Reset();

            isInitialized = true;
        }

        internal static void Update(World world)
        {
            ClampCameraInTheWorld(world);
        }

        internal static void Move(Vector2 direction)
        {
            Position += Vector2.Transform(direction, Matrix.CreateRotationZ(-Rotation));
        }

        internal static void Rotate(float deltaRadians)
        {
            Rotation += deltaRadians;
        }

        internal static void ZoomIn(float deltaZoom)
        {
            ClampZoom(Zoom + deltaZoom);
        }

        internal static void ZoomOut(float deltaZoom)
        {
            ClampZoom(Zoom - deltaZoom);
        }

        internal static void ClampZoom(float value)
        {
            Zoom = value < MinimumZoom ? MinimumZoom : value > MaximumZoom ? MaximumZoom : value;
        }

        internal static Vector2 WorldToScreen(Vector2 worldPosition)
        {
            Viewport viewport = videoManager.Viewport;
            return Vector2.Transform(worldPosition + new Vector2(viewport.X, viewport.Y), GetViewMatrix());
        }

        internal static Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            Viewport viewport = videoManager.Viewport;
            return Vector2.Transform(screenPosition - new Vector2(viewport.X, viewport.Y), Matrix.Invert(GetViewMatrix()));
        }

        internal static Matrix GetViewMatrix()
        {
            return GetVirtualViewMatrix() * Matrix.Identity;
        }

        private static Matrix GetVirtualViewMatrix()
        {
            return Matrix.CreateTranslation(new(-Position.X, Position.Y, 0.0f)) *
                   Matrix.CreateTranslation(new(-Origin, 0.0f)) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(Zoom, Zoom, 1) *
                   Matrix.CreateTranslation(new(Origin, 0.0f));
        }

        internal static bool InsideCameraBounds(Vector2 targetPosition, Point targetSize, bool inWorldPosition, float toleranceFactor = 0.0f)
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

            return screenBottomRight.X >= 0 && screenTopLeft.X < videoManager.Viewport.Width &&
                   screenBottomRight.Y >= 0 && screenTopLeft.Y < videoManager.Viewport.Height;
        }

        private static void ClampCameraInTheWorld(World world)
        {
            int totalWorldWidth = world.Information.Size.X * WorldConstants.GRID_SIZE;
            int totalWorldHeight = world.Information.Size.Y * WorldConstants.GRID_SIZE;

            float visibleWidth = ScreenConstants.SCREEN_WIDTH;
            float visibleHeight = ScreenConstants.SCREEN_HEIGHT;

            float worldLeftLimit = 0f;
            float worldRightLimit = totalWorldWidth - visibleWidth;

            float worldBottomLimit = (totalWorldHeight - visibleHeight) * -1;
            float worldTopLimit = 0f;

            Vector2 cameraPosition = SSCamera.Position;

            cameraPosition.X = MathHelper.Clamp(cameraPosition.X, worldLeftLimit, worldRightLimit);
            cameraPosition.Y = MathHelper.Clamp(cameraPosition.Y, worldBottomLimit, worldTopLimit);

            SSCamera.Position = cameraPosition;
        }
    }
}
