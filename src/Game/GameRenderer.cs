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

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Core;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.InputSystem;
using StardustSandbox.InputSystem.Game;
using StardustSandbox.IO;
using StardustSandbox.Managers;
using StardustSandbox.Serialization;
using StardustSandbox.Serialization.Settings;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox
{
    internal static class GameRenderer
    {
        internal static RenderTarget2D ScreenRenderTarget2D => screenRenderTarget2D;

        private static bool isInitialized;
        private static bool isUnloaded;
        private static bool hasScreenshotRequest;

        private static RenderTarget2D screenRenderTarget2D;
        private static RenderTarget2D backgroundRenderTarget2D;
        private static RenderTarget2D uiRenderTarget2D;
        private static RenderTarget2D worldRenderTarget2D;
        private static RenderTarget2D screenshotRenderTarget2D;
        private static RenderTarget2D cursorRenderTarget2D;

        private static GraphicsDevice graphicsDevice;

        private static RenderTarget2D CreateRenderTarget(int width, int height)
        {
            return new RenderTarget2D(
                graphicsDevice,
                width,
                height,
                false,
                SurfaceFormat.Color,
                DepthFormat.None,
                0,
                RenderTargetUsage.DiscardContents,
                false
            );
        }

        private static void DisposeRenderTarget(ref RenderTarget2D renderTarget)
        {
            renderTarget?.Dispose();
            renderTarget = null;
        }

        internal static void Initialize(VideoManager videoManager)
        {
            if (isInitialized)
            {
                throw new InvalidOperationException($"{nameof(GameRenderer)} has already been initialized.");
            }

            graphicsDevice = videoManager.GraphicsDevice;

            int width = ScreenConstants.SCREEN_WIDTH;
            int height = ScreenConstants.SCREEN_HEIGHT;

            screenRenderTarget2D = CreateRenderTarget(width, height);
            uiRenderTarget2D = CreateRenderTarget(width, height);
            backgroundRenderTarget2D = CreateRenderTarget(width, height);
            worldRenderTarget2D = CreateRenderTarget(width, height);
            screenshotRenderTarget2D = CreateRenderTarget(width, height);
            cursorRenderTarget2D = CreateRenderTarget(width, height);

            isInitialized = true;
            isUnloaded = false;
        }

        internal static void Unload()
        {
            if (isUnloaded)
            {
                throw new InvalidOperationException($"{nameof(GameRenderer)} has already been unloaded.");
            }

            DisposeRenderTarget(ref screenRenderTarget2D);
            DisposeRenderTarget(ref uiRenderTarget2D);
            DisposeRenderTarget(ref backgroundRenderTarget2D);
            DisposeRenderTarget(ref worldRenderTarget2D);
            DisposeRenderTarget(ref screenshotRenderTarget2D);
            DisposeRenderTarget(ref cursorRenderTarget2D);

            isUnloaded = true;
            isInitialized = false;
        }

        internal static void Draw(
            ActorManager actorManager,
            AmbientManager ambientManager,
            CursorManager cursorManager,
            InputController inputController,
            SpriteBatch spriteBatch,
            UIManager uiManager,
            VideoManager videoManager,
            World world
        )
        {
            if (!isInitialized || isUnloaded)
            {
                throw new InvalidOperationException($"{nameof(GameRenderer)} is not properly initialized or has been unloaded.");
            }

            DrawAmbient(spriteBatch, ambientManager);
            DrawWorld(spriteBatch, actorManager, world);
            DrawCursorPenActionArea(spriteBatch, inputController);
            DrawGUI(spriteBatch, uiManager);

            graphicsDevice.SetRenderTarget(screenRenderTarget2D);
            graphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            spriteBatch.Draw(backgroundRenderTarget2D, Vector2.Zero, Color.White);
            spriteBatch.Draw(worldRenderTarget2D, Vector2.Zero, Color.White);
            spriteBatch.Draw(cursorRenderTarget2D, Vector2.Zero, Color.White);
            spriteBatch.Draw(uiRenderTarget2D, Vector2.Zero, Color.White);
            spriteBatch.End();

            if (hasScreenshotRequest)
            {
                graphicsDevice.SetRenderTarget(screenshotRenderTarget2D);
                graphicsDevice.Clear(Color.Transparent);

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
                spriteBatch.Draw(backgroundRenderTarget2D, Vector2.Zero, Color.White);
                spriteBatch.Draw(worldRenderTarget2D, Vector2.Zero, Color.White);
                spriteBatch.Draw(cursorRenderTarget2D, Vector2.Zero, Color.White);
                spriteBatch.Draw(uiRenderTarget2D, Vector2.Zero, Color.White);
                spriteBatch.End();

                _ = SSFile.WriteRenderTarget2D(screenRenderTarget2D);
                hasScreenshotRequest = false;
            }

            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            spriteBatch.Draw(screenRenderTarget2D, videoManager.AdjustRenderTargetOnScreen(screenRenderTarget2D), Color.White);
            cursorManager.Draw(spriteBatch);
            spriteBatch.End();
        }

        private static void DrawAmbient(SpriteBatch spriteBatch, AmbientManager ambientManager)
        {
            graphicsDevice.SetRenderTarget(backgroundRenderTarget2D);
            graphicsDevice.Clear(Color.Transparent);

            // Sky
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, AssetDatabase.GetEffect(EffectIndex.GradientTransition), null);
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Pixel), Vector2.Zero, null, AAP64ColorPalette.White, 0f, Vector2.Zero, new Vector2(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT), SpriteEffects.None, 0f);
            spriteBatch.End();

            // Celestial Bodies
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            ambientManager.CelestialBodyHandler.Draw(spriteBatch);
            spriteBatch.End();

            // Background
            if (ambientManager.BackgroundHandler.IsAffectedByLighting)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, AssetDatabase.GetEffect(EffectIndex.GradientTransition), null);
                ambientManager.CloudHandler.Draw(spriteBatch);
                ambientManager.BackgroundHandler.Draw(spriteBatch);
                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
                ambientManager.CloudHandler.Draw(spriteBatch);
                ambientManager.BackgroundHandler.Draw(spriteBatch);
                spriteBatch.End();
            }
        }

        private static void DrawWorld(SpriteBatch spriteBatch, ActorManager actorManager, World world)
        {
            graphicsDevice.SetRenderTarget(worldRenderTarget2D);
            graphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, SSCamera.GetViewMatrix());
            world.Draw(spriteBatch);
            actorManager.Draw(spriteBatch);
            spriteBatch.End();
        }

        private static void DrawGUI(SpriteBatch spriteBatch, UIManager uiManager)
        {
            graphicsDevice.SetRenderTarget(uiRenderTarget2D);
            graphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            uiManager.Draw(spriteBatch);
            spriteBatch.End();
        }

        private static void DrawCursorPenActionArea(SpriteBatch spriteBatch, InputController inputController)
        {
            graphicsDevice.SetRenderTarget(cursorRenderTarget2D);
            graphicsDevice.Clear(Color.Transparent);

            GameplaySettings gameplaySettings = SettingsSerializer.Load<GameplaySettings>();

            PenTool penTool = inputController.Pen.Tool;

            if (!gameplaySettings.ShowPreviewArea || penTool is PenTool.Visualization or PenTool.Fill)
            {
                return;
            }

            Vector2 mousePosition = Input.GetScaledMousePosition();
            Vector2 worldMousePosition = SSCamera.ScreenToWorld(mousePosition);

            Point alignedPosition = new(
                (int)Math.Floor(worldMousePosition.X / WorldConstants.GRID_SIZE),
                (int)Math.Floor(worldMousePosition.Y / WorldConstants.GRID_SIZE)
            );

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            foreach (Point point in inputController.Pen.GetShapePoints(alignedPosition))
            {
                Vector2 worldPosition = new(
                    point.X * WorldConstants.GRID_SIZE,
                    point.Y * WorldConstants.GRID_SIZE
                );

                Vector2 screenPosition = SSCamera.WorldToScreen(worldPosition);

                spriteBatch.Draw(
                    AssetDatabase.GetTexture(TextureIndex.ShapeSquares),
                    screenPosition,
                    new(110, 0, 32, 32),
                    gameplaySettings.PreviewAreaColor,
                    0f,
                    Vector2.Zero,
                    Vector2.One,
                    SpriteEffects.None,
                    0f
                );
            }

            spriteBatch.End();
        }

        internal static Vector2 CalculateScaledMousePosition(in Vector2 mousePosition, VideoManager videoManager)
        {
            Rectangle adjustedScreen = videoManager.AdjustRenderTargetOnScreen(ScreenRenderTarget2D);

            float scale = adjustedScreen.Width / (float)ScreenRenderTarget2D.Width;

            float mouseX = (mousePosition.X - adjustedScreen.X) / scale;
            float mouseY = (mousePosition.Y - adjustedScreen.Y) / scale;

            mouseX = Math.Clamp(mouseX, 0, ScreenRenderTarget2D.Width - 1);
            mouseY = Math.Clamp(mouseY, 0, ScreenRenderTarget2D.Height - 1);

            return new(mouseX, mouseY);
        }

        internal static void RequestScreenshot()
        {
            hasScreenshotRequest = true;
        }
    }
}

