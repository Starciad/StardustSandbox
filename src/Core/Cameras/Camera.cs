using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.WorldSystem;

using System;

namespace StardustSandbox.Core.Cameras
{
    internal static class Camera
    {
        internal static Vector2 Position => position;

        private static Vector2 position;
        private static Vector2 targetPosition;

        private static World world;
        private static bool isInitialized = false;

        internal static void Reset()
        {
            SetPosition(Vector2.Zero);
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

        private static void ClampInTheWorld()
        {
            Vector2 worldSize = new(world.Information.Size.X * WorldConstants.GRID_SIZE, world.Information.Size.Y * WorldConstants.GRID_SIZE);
            Vector2 screenSize = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT);
            RectangleF limit = new(0f, 0f, worldSize.X - screenSize.X, screenSize.Y - worldSize.Y);

            Clamp(ref position);
            Clamp(ref targetPosition);

            void Clamp(ref Vector2 value)
            {
                value = new(
                    MathHelper.Clamp(value.X, limit.Left, limit.Right),
                    MathHelper.Clamp(value.Y, limit.Bottom, limit.Top)
                );
            }
        }

        internal static void Update(GameTime gameTime)
        {
            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            position = Vector2.Lerp(Position, targetPosition, CameraConstants.MOVEMENT_LERP_SPEED * deltaTime);

            ClampInTheWorld();
        }

        internal static void SetPosition(Vector2 newPosition)
        {
            position = newPosition;
            targetPosition = newPosition;
        }

        internal static void Move(Vector2 direction)
        {
            targetPosition += direction;
        }

        private static Matrix GetVirtualViewMatrix()
        {
            return Matrix.CreateTranslation(new(-Position.X, Position.Y, 0.0f));
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

        internal static bool IsWithinBounds(RectangleF rect, bool isWorldPosition, float toleranceFactor = 0.0f)
        {
            // Expand the rectangle by toleranceFactor (in all 4 directions)
            if (toleranceFactor != 0f)
            {
                rect = new(
                    rect.X - toleranceFactor,
                    rect.Y - toleranceFactor,
                    rect.Width + (2f * toleranceFactor),
                    rect.Height + (2f * toleranceFactor)
                );
            }

            // Convert to screen coordinates if required
            RectangleF screenRect;

            if (isWorldPosition)
            {
                // Convert top-left and bottom-right corners and build a normalized rectangle
                Vector2 worldTL = new(rect.X, rect.Y);
                Vector2 worldBR = new(rect.X + rect.Width, rect.Y + rect.Height);

                Vector2 screenTL = WorldToScreen(worldTL);
                Vector2 screenBR = WorldToScreen(worldBR);

                float left = Math.Min(screenTL.X, screenBR.X);
                float right = Math.Max(screenTL.X, screenBR.X);
                float top = Math.Min(screenTL.Y, screenBR.Y);
                float bottom = Math.Max(screenTL.Y, screenBR.Y);

                screenRect = new(left, top, right - left, bottom - top);
            }
            else
            {
                // Already in screen coordinates — ensure positive width/height
                float left = Math.Min(rect.X, rect.X + rect.Width);
                float right = Math.Max(rect.X, rect.X + rect.Width);
                float top = Math.Min(rect.Y, rect.Y + rect.Height);
                float bottom = Math.Max(rect.Y, rect.Y + rect.Height);

                screenRect = new(left, top, right - left, bottom - top);
            }

            // Screen rectangle bounds
            float screenLeft = 0f;
            float screenTop = 0f;
            float screenRight = ScreenConstants.SCREEN_WIDTH;
            float screenBottom = ScreenConstants.SCREEN_HEIGHT;

            // Check overlap (any intersection with the screen area)
            bool intersectsHorizontally = screenRect.X + screenRect.Width >= screenLeft && screenRect.X < screenRight;
            bool intersectsVertically = screenRect.Y + screenRect.Height >= screenTop && screenRect.Y < screenBottom;

            return intersectsHorizontally && intersectsVertically;
        }
    }
}
