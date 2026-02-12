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
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Cameras;
using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Inputs.Game;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.IO;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;
using StardustSandbox.Core.WorldSystem;

using System;

namespace StardustSandbox.Core
{
    internal static class GameRenderer
    {
        private static bool isInitialized;
        private static bool hasScreenshotRequest;

        private static GraphicsDevice graphicsDevice;

        internal static void Initialize(VideoManager videoManager)
        {
            if (isInitialized)
            {
                throw new InvalidOperationException($"{nameof(GameRenderer)} has already been initialized.");
            }

            graphicsDevice = videoManager.GraphicsDevice;
            isInitialized = true;
        }

        internal static void Draw(
            ActorManager actorManager,
            AmbientManager ambientManager,
            CursorManager cursorManager,
            PlayerInputController inputController,
            SpriteBatch spriteBatch,
            UIManager uiManager,
            World world
        )
        {
            if (!isInitialized)
            {
                throw new InvalidOperationException($"{nameof(GameRenderer)} is not initialized.");
            }

            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Clear(Color.Transparent);

            DrawAmbient(spriteBatch, ambientManager);
            DrawWorld(spriteBatch, actorManager, world);
            DrawCursorPenActionArea(spriteBatch, inputController);
            DrawGUI(spriteBatch, uiManager);
            DrawCursor(spriteBatch, cursorManager);

            if (hasScreenshotRequest)
            {
                SaveBackBufferScreenshot();
                hasScreenshotRequest = false;
            }
        }

        private static void DrawAmbient(SpriteBatch spriteBatch, AmbientManager ambientManager)
        {
            // Sky (gradient)
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                null,
                null,
                null,
                AssetDatabase.GetEffect(EffectIndex.GradientTransition)
            );

            spriteBatch.Draw(
                AssetDatabase.GetTexture(TextureIndex.Pixel),
                new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height),
                AAP64ColorPalette.White
            );

            spriteBatch.End();

            // Celestial bodies + background
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp
            );

            ambientManager.CelestialBodyHandler.Draw(spriteBatch);
            ambientManager.CloudHandler.Draw(spriteBatch);
            ambientManager.BackgroundHandler.Draw(spriteBatch);

            spriteBatch.End();
        }

        private static void DrawWorld(SpriteBatch spriteBatch, ActorManager actorManager, World world)
        {
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullNone,
                null,
                Camera.GetViewMatrix()
            );

            world.Draw(spriteBatch);
            actorManager.Draw(spriteBatch);

            spriteBatch.End();
        }

        private static void DrawGUI(SpriteBatch spriteBatch, UIManager uiManager)
        {
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp
            );

            uiManager.Draw(spriteBatch);

            spriteBatch.End();
        }

        private static void DrawCursor(SpriteBatch spriteBatch, CursorManager cursorManager)
        {
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp
            );

            cursorManager.Draw(spriteBatch);

            spriteBatch.End();
        }

        private static void DrawCursorPenActionArea(SpriteBatch spriteBatch, PlayerInputController inputController)
        {
            GameplaySettings gameplaySettings = SettingsSerializer.Load<GameplaySettings>();
            PenTool penTool = inputController.Pen.Tool;

            if (!gameplaySettings.ShowPreviewArea || penTool is PenTool.Visualization or PenTool.Fill)
            {
                return;
            }

            Vector2 mousePosition = InputEngine.GetMousePosition();
            Vector2 worldMousePosition = Camera.ScreenToWorld(mousePosition);

            Point alignedPosition = new(
                (int)Math.Floor(worldMousePosition.X / WorldConstants.GRID_SIZE),
                (int)Math.Floor(worldMousePosition.Y / WorldConstants.GRID_SIZE)
            );

            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp
            );

            foreach (Point point in inputController.Pen.GetShapePoints(alignedPosition))
            {
                Vector2 worldPosition = new(
                    point.X * WorldConstants.GRID_SIZE,
                    point.Y * WorldConstants.GRID_SIZE
                );

                Vector2 screenPosition = Camera.WorldToScreen(worldPosition);

                spriteBatch.Draw(
                    AssetDatabase.GetTexture(TextureIndex.ShapeSquares),
                    screenPosition,
                    new Rectangle(110, 0, 32, 32),
                    gameplaySettings.PreviewAreaColor
                );
            }

            spriteBatch.End();
        }

        private static void SaveBackBufferScreenshot()
        {
            int width = graphicsDevice.PresentationParameters.BackBufferWidth;
            int height = graphicsDevice.PresentationParameters.BackBufferHeight;

            Color[] data = new Color[width * height];
            graphicsDevice.GetBackBufferData(data);

            File.WriteColorBuffer(graphicsDevice, width, height, data);
        }

        internal static void RequestScreenshot()
        {
            hasScreenshotRequest = true;
        }
    }
}

