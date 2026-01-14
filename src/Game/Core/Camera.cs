using Microsoft.Xna.Framework;

using StardustSandbox.Constants;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.Core
{
    internal static class Camera
    {
        internal static Vector2 Position { get; private set; }
        internal static Vector2 Origin { get; set; }

        internal static float Zoom
        {
            get => zoom;
            private set
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

                minimumZoom = value;

                if (targetZoom < minimumZoom)
                {
                    targetZoom = minimumZoom;
                }

                if (Zoom < minimumZoom)
                {
                    Zoom = minimumZoom;
                }
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

                maximumZoom = value;

                if (targetZoom > maximumZoom)
                {
                    targetZoom = maximumZoom;
                }

                if (Zoom > maximumZoom)
                {
                    Zoom = maximumZoom;
                }
            }
        }

        private static float maximumZoom = float.MaxValue;
        private static float minimumZoom = 0.0f;
        private static float zoom;

        private static Vector2 targetPosition;
        private static float targetZoom;

        private static World world;

        private static bool isInitialized = false;

        internal static void Reset()
        {
            Zoom = 1.0f;
            targetZoom = Zoom;

            Origin = new(ScreenConstants.SCREEN_WIDTH / 2.0f, ScreenConstants.SCREEN_HEIGHT / 2.0f);
            Position = Vector2.Zero;
            targetPosition = Position;
        }

        internal static void Initialize(World world)
        {
            if (isInitialized)
            {
                throw new InvalidOperationException($"{nameof(Camera)} is already initialized");
            }

            Camera.world = world;

            Reset();

            isInitialized = true;
        }

        internal static void Update(GameTime gameTime)
        {
            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            // Compute interpolation alpha using an exponential smoothing formulation:
            // alpha = 1 - exp(-speed * dt). This yields frame-rate independent smoothing.
            float posAlpha = 1f - (float)Math.Exp(-CameraConstants.MOVEMENT_LERP_SPEED * deltaTime);
            float zoomAlpha = 1f - (float)Math.Exp(-CameraConstants.ZOOM_LERP_SPEED * deltaTime);

            Position = Vector2.Lerp(Position, targetPosition, MathHelper.Clamp(posAlpha, 0f, 1f));
            Zoom = MathHelper.Lerp(Zoom, targetZoom, MathHelper.Clamp(zoomAlpha, 0f, 1f));

            // Use the property setter to validate ranges
            ClampCameraInTheWorld();
        }

        internal static void Move(Vector2 direction)
        {
            targetPosition += direction;
        }

        internal static void MoveInstant(Vector2 newPosition)
        {
            Position = newPosition;
            targetPosition = newPosition;
        }

        internal static void ZoomIn(float deltaZoom)
        {
            ClampZoom(targetZoom + deltaZoom);
        }

        internal static void ZoomOut(float deltaZoom)
        {
            ClampZoom(targetZoom - deltaZoom);
        }

        internal static void ClampZoom(float value)
        {
            float clamped = value < MinimumZoom ? MinimumZoom : value > MaximumZoom ? MaximumZoom : value;
            targetZoom = clamped;
        }

        internal static void ClampZoomInstant(float value)
        {
            float clamped = value < MinimumZoom ? MinimumZoom : value > MaximumZoom ? MaximumZoom : value;
            targetZoom = clamped;
            Zoom = clamped;
        }

        private static Matrix GetVirtualViewMatrix()
        {
            return Matrix.CreateTranslation(new(-Position.X, Position.Y, 0.0f)) *
                   Matrix.CreateTranslation(new(-Origin, 0.0f)) *
                   Matrix.CreateScale(Zoom, Zoom, 1.0f) *
                   Matrix.CreateTranslation(new(Origin, 0.0f));
        }

        internal static Matrix GetViewMatrix()
        {
            return GetVirtualViewMatrix() * Matrix.Identity;
        }

        internal static Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, GetViewMatrix());
        }

        internal static Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(GetViewMatrix()));
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

            return screenBottomRight.X >= 0 && screenTopLeft.X < ScreenConstants.SCREEN_WIDTH &&
                   screenBottomRight.Y >= 0 && screenTopLeft.Y < ScreenConstants.SCREEN_HEIGHT;
        }

        private static void ClampCameraInTheWorld()
        {
            int totalWorldWidth = world.Information.Size.X * WorldConstants.GRID_SIZE;
            int totalWorldHeight = world.Information.Size.Y * WorldConstants.GRID_SIZE;

            // Visible area in world units must account for zoom (screen pixels scaled by zoom)
            float visibleWidth = ScreenConstants.SCREEN_WIDTH / Zoom;
            float visibleHeight = ScreenConstants.SCREEN_HEIGHT / Zoom;

            float worldLeftLimit = 0f;
            float worldRightLimit = Math.Max(0f, totalWorldWidth - visibleWidth);

            float worldTopLimit = 0f;
            float worldBottomLimit = Math.Min(0f, visibleHeight - totalWorldHeight); // keep same sign convention as original

            Vector2 cameraPosition = Position;

            cameraPosition.X = MathHelper.Clamp(cameraPosition.X, worldLeftLimit, worldRightLimit);
            cameraPosition.Y = MathHelper.Clamp(cameraPosition.Y, worldBottomLimit, worldTopLimit);

            Position = cameraPosition;

            // Also ensure target position respects same clamp, so smoothing doesn't try to reach out-of-bounds target
            targetPosition.X = MathHelper.Clamp(targetPosition.X, worldLeftLimit, worldRightLimit);
            targetPosition.Y = MathHelper.Clamp(targetPosition.Y, worldBottomLimit, worldTopLimit);
        }
    }
}
