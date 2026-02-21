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

using System;

namespace StardustSandbox.Core.Cameras
{
    internal static class Camera
    {
        internal static Vector2 Position => position;
        internal static float Zoom => zoom;

        private static Vector2 position;
        private static Vector2 targetPosition;

        private static float zoom;
        private static float targetZoom;

        private static bool isInitialized = false;

        internal static void Reset()
        {
            SetPosition(Vector2.Zero);
            SetZoom(1.0f);
        }

        internal static void Initialize()
        {
            if (isInitialized)
            {
                throw new InvalidOperationException($"{nameof(Camera)} is already initialized");
            }

            Reset();

            isInitialized = true;
        }

        internal static void Update(GameTime gameTime)
        {
            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            position = Vector2.Lerp(Position, targetPosition, CameraConstants.MOVEMENT_LERP_SPEED * deltaTime);
            zoom = MathHelper.Lerp(zoom, targetZoom, CameraConstants.ZOOM_LERP_SPEED * deltaTime);
        }

        internal static void SetPosition(Vector2 newPosition)
        {
            position = newPosition;
            targetPosition = newPosition;
        }

        internal static void SetZoom(float newZoom)
        {
            zoom = newZoom;
            targetZoom = newZoom;
        }

        internal static void MoveUp(float amount)
        {
            targetPosition += new Vector2(0.0f, -amount);
        }

        internal static void MoveDown(float amount)
        {
            targetPosition += new Vector2(0.0f, amount);
        }

        internal static void MoveLeft(float amount)
        {
            targetPosition += new Vector2(-amount, 0.0f);
        }

        internal static void MoveRight(float amount)
        {
            targetPosition += new Vector2(amount, 0.0f);
        }

        internal static void FadeIn(float amount)
        {
            targetZoom += amount;
            targetZoom = Math.Min(targetZoom, CameraConstants.MAX_ZOOM);
        }

        internal static void FadeOut(float amount)
        {
            targetZoom -= amount;
            targetZoom = Math.Max(targetZoom, CameraConstants.MIN_ZOOM);
        }

        private static Matrix GetVirtualViewMatrix()
        {
            Vector2 viewportCenter = GameScreen.GetViewportCenter();

            return
                Matrix.CreateTranslation(-Position.X, -Position.Y, 0f) *
                Matrix.CreateScale(zoom, zoom, 1f) *
                Matrix.CreateTranslation(viewportCenter.X, viewportCenter.Y, 0f);
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

        internal static RectangleF GetViewBounds()
        {
            Vector2 viewportSize = GameScreen.GetViewport();
            
            Vector2 topLeft = ScreenToWorld(Vector2.Zero);
            Vector2 bottomRight = ScreenToWorld(viewportSize);

            float width = bottomRight.X - topLeft.X;
            float height = bottomRight.Y - topLeft.Y;

            return new(topLeft.X, topLeft.Y, width, height);
        }
    }
}
