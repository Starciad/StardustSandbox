/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

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

        internal static void Update(GameTime gameTime)
        {
            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            position = Vector2.Lerp(Position, targetPosition, CameraConstants.MOVEMENT_LERP_SPEED * deltaTime);
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
                // Already in screen coordinates � ensure positive width/height
                float left = Math.Min(rect.X, rect.X + rect.Width);
                float right = Math.Max(rect.X, rect.X + rect.Width);
                float top = Math.Min(rect.Y, rect.Y + rect.Height);
                float bottom = Math.Max(rect.Y, rect.Y + rect.Height);

                screenRect = new(left, top, right - left, bottom - top);
            }

            // Screen rectangle bounds
            float screenLeft = 0f;
            float screenTop = 0f;

            // Check overlap (any intersection with the screen area)
            Vector2 viewport = GameScreen.GetViewport();

            bool intersectsHorizontally = screenRect.X + screenRect.Width >= screenLeft && screenRect.X < viewport.X;
            bool intersectsVertically = screenRect.Y + screenRect.Height >= screenTop && screenRect.Y < viewport.Y;

            return intersectsHorizontally && intersectsVertically;
        }
    }
}
