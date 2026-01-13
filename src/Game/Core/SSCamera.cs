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

using StardustSandbox.Constants;
using StardustSandbox.Managers;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.Core
{
    internal static class SSCamera
    {
        internal static Vector2 Position { get; set; }
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

        private static Vector2 targetPosition;
        private static float targetZoom;

        private static VideoManager videoManager;
        private static World world;

        private static bool isInitialized = false;

        private const float MOVE_LERP_SPEED = 10.0f;
        private const float ZOOM_LERP_SPEED = 8.0f;

        internal static void Reset()
        {
            Zoom = 1.0f;
            targetZoom = Zoom;

            Origin = new(ScreenConstants.SCREEN_WIDTH / 2.0f,
                         ScreenConstants.SCREEN_HEIGHT / 2.0f);

            Position = Vector2.Zero;
            targetPosition = Position;
        }

        internal static void Initialize(VideoManager videoManager, World world)
        {
            if (isInitialized)
            {
                throw new InvalidOperationException($"{nameof(SSCamera)} is already initialized");
            }

            SSCamera.videoManager = videoManager;
            SSCamera.world = world;

            Reset();

            isInitialized = true;
        }

        private static void ClampTargetPositionInWorld(World world)
        {
            targetPosition = ClampPositionInWorld(targetPosition, world);
        }

        internal static void Update(GameTime gameTime)
        {
            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            // Ensure zoom limits based on world size
            RecalculateZoomLimitsFromWorld(world);

            // Smooth zoom
            Zoom = MathHelper.Lerp(
                Zoom,
                targetZoom,
                1f - MathF.Exp(-ZOOM_LERP_SPEED * deltaTime)
            );

            // Clamp only the target
            ClampTargetPositionInWorld(world);

            // Smooth movement
            Position = Vector2.Lerp(
                Position,
                targetPosition,
                1f - MathF.Exp(-MOVE_LERP_SPEED * deltaTime)
            );
        }

        internal static void Move(Vector2 direction)
        {
            targetPosition += Vector2.Transform(direction, Matrix.CreateRotationZ(0.0f));
        }

        private static void SetTargetZoom(float value)
        {
            targetZoom = MathHelper.Clamp(value, MinimumZoom, MaximumZoom);
        }

        internal static void ZoomIn(float deltaZoom)
        {
            SetTargetZoom(targetZoom + deltaZoom);
        }

        internal static void ZoomOut(float deltaZoom)
        {
            SetTargetZoom(targetZoom - deltaZoom);
        }

        internal static void ClampZoom(float value)
        {
            Zoom = value < MinimumZoom ? MinimumZoom : value > MaximumZoom ? MaximumZoom : value;
        }

        internal static Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(new(worldPosition.X + videoManager.Viewport.X, worldPosition.Y + videoManager.Viewport.Y), GetViewMatrix());
        }

        internal static Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(new(screenPosition.X - videoManager.Viewport.X, screenPosition.Y - videoManager.Viewport.Y), Matrix.Invert(GetViewMatrix()));
        }

        internal static Matrix GetViewMatrix()
        {
            return GetVirtualViewMatrix() * Matrix.Identity;
        }

        private static Matrix GetVirtualViewMatrix()
        {
            return Matrix.CreateTranslation(new(-Position.X, Position.Y, 0.0f)) *
                   Matrix.CreateTranslation(new(-Origin, 0.0f)) *
                   Matrix.CreateRotationZ(0.0f) *
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

            return screenBottomRight.X >= 0 && screenTopLeft.X < ScreenConstants.SCREEN_WIDTH &&
                   screenBottomRight.Y >= 0 && screenTopLeft.Y < ScreenConstants.SCREEN_HEIGHT;
        }

        private static void RecalculateZoomLimitsFromWorld(World world)
        {
            int totalWorldWidth = world.Information.Size.X * WorldConstants.GRID_SIZE;
            int totalWorldHeight = world.Information.Size.Y * WorldConstants.GRID_SIZE;

            // Proteções simples para evitar divisão por zero
            if (totalWorldWidth <= 0 || totalWorldHeight <= 0)
            {
                return;
            }

            // Para que a área visível nunca seja maior que o mundo:
            // ScreenWidth / Zoom <= totalWorldWidth  =>  Zoom >= ScreenWidth / totalWorldWidth
            float requiredZoomForWidth = ScreenConstants.SCREEN_WIDTH / (float)totalWorldWidth;
            float requiredZoomForHeight = ScreenConstants.SCREEN_HEIGHT / (float)totalWorldHeight;

            // mínimo aceitável para Zoom (impede "zoom out" que mostra espaço vazio)
            float minAllowedZoom = Math.Max(requiredZoomForWidth, requiredZoomForHeight);

            // Ajusta MinimumZoom para este mundo (preservando a regra de não-negatividade)
            // Se você preferir não sobrescrever uma MinimumZoom já configurada externamente,
            // use Math.Max(MinimumZoom, minAllowedZoom) em vez de atribuir diretamente.
            MinimumZoom = Math.Max(MinimumZoom, minAllowedZoom);

            // Caso atual do Zoom esteja menor que o novo mínimo, force o ajuste
            if (Zoom < MinimumZoom)
            {
                Zoom = MinimumZoom;
            }
        }

        private static Vector2 ClampPositionInWorld(Vector2 position, World world)
        {
            int totalWorldWidth = world.Information.Size.X * WorldConstants.GRID_SIZE;
            int totalWorldHeight = world.Information.Size.Y * WorldConstants.GRID_SIZE;

            float visibleWidth = ScreenConstants.SCREEN_WIDTH / Zoom;
            float visibleHeight = ScreenConstants.SCREEN_HEIGHT / Zoom;

            float maxPosX = Math.Max(0f, totalWorldWidth - visibleWidth);
            float maxPosY = Math.Max(0f, totalWorldHeight - visibleHeight);

            position.X = MathHelper.Clamp(position.X, 0f, maxPosX);
            position.Y = MathHelper.Clamp(position.Y, 0f, maxPosY);

            return position;
        }
    }
}

