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

using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Inputs.Game;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.IO;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;

using System;

namespace StardustSandbox.Core
{
    public sealed partial class StardustSandboxGame
    {
        private bool hasScreenshotRequest;

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.SetRenderTarget(null);
            this.GraphicsDevice.Clear(Color.Transparent);

            DrawAmbient();
            DrawWorld();
            DrawCursorPenActionArea();
            DrawGUI();
            DrawCursor();

            if (this.hasScreenshotRequest)
            {
                SaveBackBufferScreenshot();
                this.hasScreenshotRequest = false;
            }

            base.Draw(gameTime);
        }

        private void DrawAmbient()
        {
            Effect gradientTransitionEffect = AssetDatabase.GetEffect(EffectIndex.GradientTransition);

            // Sky (gradient)
            this.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                null,
                null,
                null,
                gradientTransitionEffect
            );
            this.spriteBatch.Draw(
                AssetDatabase.GetTexture(TextureIndex.Pixel),
                new Rectangle(0, 0, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height),
                AAP64ColorPalette.White
            );
            this.spriteBatch.End();

            // Celestial bodies
            this.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp
            );
            this.ambientManager.CelestialBodyHandler.Draw(this.spriteBatch);
            this.spriteBatch.End();

            // Background
            this.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullNone,
                this.ambientManager.BackgroundHandler.GetCurrentBackground().IsAffectedByLighting ? gradientTransitionEffect : null,
                null
            );
            this.ambientManager.BackgroundHandler.Draw(this.spriteBatch, this.camera);
            this.spriteBatch.End();
        }

        private void DrawWorld()
        {
            this.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullNone,
                null,
                this.camera.GetViewMatrix()
            );

            this.world.Draw(this.spriteBatch, this.camera, this.gameLaunchOptions);
            this.actorManager.Draw(this.spriteBatch, this.camera);

            this.spriteBatch.End();
        }

        private void DrawGUI()
        {
            this.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp
            );

            this.uiManager.Draw(this.spriteBatch);

            this.spriteBatch.End();
        }

        private void DrawCursor()
        {
            this.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp
            );

            this.cursorManager.Draw(this.spriteBatch);

            this.spriteBatch.End();
        }

        private void DrawCursorPenActionArea()
        {
            GameplaySettings gameplaySettings = SettingsSerializer.Load<GameplaySettings>();

            if (!gameplaySettings.ShowPreviewArea || this.playerInputController.Pen.Tool is PenTool.Visualization or PenTool.Fill)
            {
                return;
            }

            Vector2 screenMousePosition = InputEngine.GetCurrentMousePosition();
            Vector2 worldMousePosition = this.camera.ScreenToWorld(screenMousePosition);

            Point alignedPosition = new(
                (int)Math.Floor(worldMousePosition.X / WorldConstants.TILE_SIZE),
                (int)Math.Floor(worldMousePosition.Y / WorldConstants.TILE_SIZE)
            );

            this.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp,
                transformMatrix: this.camera.GetViewMatrix()
            );

            foreach (Point point in this.playerInputController.Pen.GetShapePoints(alignedPosition))
            {
                Vector2 worldPosition = new(
                    point.X * WorldConstants.TILE_SIZE,
                    point.Y * WorldConstants.TILE_SIZE
                );

                this.spriteBatch.Draw(
                    AssetDatabase.GetTexture(TextureIndex.ShapeSquares),
                    worldPosition,
                    new Rectangle(110, 0, 32, 32),
                    gameplaySettings.PreviewAreaColor
                );
            }

            this.spriteBatch.End();
        }

        private void SaveBackBufferScreenshot()
        {
            int width = this.GraphicsDevice.PresentationParameters.BackBufferWidth;
            int height = this.GraphicsDevice.PresentationParameters.BackBufferHeight;

            Color[] data = new Color[width * height];
            this.GraphicsDevice.GetBackBufferData(data);

            // Flatten Alpha
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new(data[i].R, data[i].G, data[i].B, (byte)255);
            }

            File.WriteColorBuffer(this.GraphicsDevice, width, height, data);
        }

        internal void RequestScreenshot()
        {
            this.hasScreenshotRequest = true;
        }
    }
}
