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
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;

using System;

namespace StardustSandbox.Core.Cameras
{
    internal sealed class Camera2D
    {
        internal Vector2 Position => this.position;
        internal float Zoom => this.zoom;

        private Vector2 position;
        private Vector2 targetPosition;

        private float zoom;
        private float targetZoom;

        private readonly GameplaySettings gameplaySettings;

        internal Camera2D()
        {
            this.gameplaySettings = SettingsSerializer.Load<GameplaySettings>();
            Reset();
        }

        internal void Reset()
        {
            SetPosition(Vector2.Zero);
            SetZoom(1.0f);
        }

        internal void Update(GameTime gameTime)
        {
            if (this.gameplaySettings.UseSmoothCameraMovement)
            {
                float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

                this.position = Vector2.Lerp(this.Position, this.targetPosition, CameraConstants.MOVEMENT_LERP_SPEED * deltaTime);
                this.zoom = MathHelper.Lerp(this.zoom, this.targetZoom, CameraConstants.ZOOM_LERP_SPEED * deltaTime);
            }
            else
            {
                this.position = this.targetPosition;
                this.zoom = this.targetZoom;
            }
        }

        internal void ClampTargetPositionToBounds(RectangleF bounds)
        {
            this.targetPosition = Vector2.Clamp(this.targetPosition, bounds.Location, bounds.Location + bounds.Size);
        }

        internal void SetPosition(Vector2 newPosition)
        {
            this.position = newPosition;
            this.targetPosition = newPosition;
        }

        internal void SetZoom(float newZoom)
        {
            this.zoom = newZoom;
            this.targetZoom = newZoom;
        }

        internal void MoveUp(float amount)
        {
            this.targetPosition += new Vector2(0.0f, -amount);
        }

        internal void MoveDown(float amount)
        {
            this.targetPosition += new Vector2(0.0f, amount);
        }

        internal void MoveLeft(float amount)
        {
            this.targetPosition += new Vector2(-amount, 0.0f);
        }

        internal void MoveRight(float amount)
        {
            this.targetPosition += new Vector2(amount, 0.0f);
        }

        internal void FadeIn(float amount)
        {
            this.targetZoom += amount;
            this.targetZoom = Math.Min(this.targetZoom, CameraConstants.MAX_ZOOM);
        }

        internal void FadeOut(float amount)
        {
            this.targetZoom -= amount;
            this.targetZoom = Math.Max(this.targetZoom, CameraConstants.MIN_ZOOM);
        }

        private Matrix GetVirtualViewMatrix()
        {
            Vector2 viewportCenter = GameScreen.GetViewportCenter();

            return
                Matrix.CreateTranslation(-this.Position.X, -this.Position.Y, 0f) *
                Matrix.CreateScale(this.zoom, this.zoom, 1f) *
                Matrix.CreateTranslation(viewportCenter.X, viewportCenter.Y, 0f);
        }

        internal Matrix GetViewMatrix()
        {
            return GetVirtualViewMatrix() * Matrix.Identity;
        }

        internal Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, GetViewMatrix());
        }

        internal Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(GetViewMatrix()));
        }

        internal RectangleF GetViewBounds()
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
