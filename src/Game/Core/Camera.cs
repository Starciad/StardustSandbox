using Microsoft.Xna.Framework;

using StardustSandbox.Constants;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.Core
{
    internal static class Camera
    {
        internal static Vector2 Position { get; private set; }
        internal static float Zoom { get; private set; }
        internal static Vector2 Origin => new(ScreenConstants.SCREEN_WIDTH / 2.0f, ScreenConstants.SCREEN_HEIGHT / 2.0f);

        private static Vector2 targetPosition;
        private static float targetZoom;

        private static World world;

        private static bool isInitialized = false;

        internal static void Reset()
        {
            SetPosition(Vector2.Zero);
            SetZoom(1.0f);
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

        private static void ClampCameraPositionInTheWorld()
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

        internal static void Update(GameTime gameTime)
        {
            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            // Smoothly interpolate position and zoom towards targets
            Position = Vector2.Lerp(Position, targetPosition, CameraConstants.MOVEMENT_LERP_SPEED * deltaTime);
            Zoom = MathHelper.Lerp(Zoom, targetZoom, CameraConstants.ZOOM_LERP_SPEED * deltaTime);

            // Use the property setter to validate ranges
            // ClampCameraPositionInTheWorld();
        }

        internal static void SetPosition(Vector2 newPosition)
        {
            Position = newPosition;
            targetPosition = newPosition;
        }

        internal static void SetZoom(float newZoom)
        {
            Zoom = newZoom;
            targetZoom = newZoom;
        }

        internal static void Move(Vector2 direction)
        {
            targetPosition += direction;
        }

        internal static void ZoomIn(float deltaZoom)
        {
            targetZoom += deltaZoom;
        }

        internal static void ZoomOut(float deltaZoom)
        {
            targetZoom -= deltaZoom;
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
    }
}
