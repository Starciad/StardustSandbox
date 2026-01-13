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

using StardustSandbox.Constants;
using StardustSandbox.Serialization;
using StardustSandbox.Serialization.Settings;

using System;

namespace StardustSandbox.Managers
{
    internal sealed class VideoManager
    {
        internal GraphicsDeviceManager GraphicsDeviceManager => this.graphicsDeviceManager;
        internal GraphicsDevice GraphicsDevice => this.graphicsDeviceManager.GraphicsDevice;
        internal GameWindow GameWindow { get; private set; }

        private readonly GraphicsDeviceManager graphicsDeviceManager;

        internal VideoManager(GraphicsDeviceManager graphicsDeviceManager)
        {
            this.graphicsDeviceManager = graphicsDeviceManager;
            ApplySettings(SettingsSerializer.Load<VideoSettings>());
        }

        internal void ApplySettings(in VideoSettings videoSettings)
        {
            _ = this.GameWindow?.IsBorderless = videoSettings.Borderless;

            if (videoSettings.Width == 0 || videoSettings.Height == 0)
            {
                this.graphicsDeviceManager.PreferredBackBufferWidth = ScreenConstants.SCREEN_WIDTH;
                this.graphicsDeviceManager.PreferredBackBufferHeight = ScreenConstants.SCREEN_HEIGHT;
            }
            else
            {
                this.graphicsDeviceManager.PreferredBackBufferWidth = videoSettings.Width;
                this.graphicsDeviceManager.PreferredBackBufferHeight = videoSettings.Height;
            }

            this.graphicsDeviceManager.IsFullScreen = videoSettings.FullScreen;
            this.graphicsDeviceManager.SynchronizeWithVerticalRetrace = videoSettings.VSync;
            this.graphicsDeviceManager.ApplyChanges();
        }

        internal void SetGameWindow(GameWindow gameWindow)
        {
            this.GameWindow = gameWindow;
        }

        internal Rectangle AdjustRenderTargetOnScreen(RenderTarget2D renderTarget)
        {
            Rectangle screenDimensions, adjustedScreen;
            float scale, newWidth, newHeight, posX, posY;

            screenDimensions = this.GraphicsDevice.PresentationParameters.Bounds;

            scale = MathF.Min(
                screenDimensions.Width / (float)renderTarget.Width,
                screenDimensions.Height / (float)renderTarget.Height
            );

            newWidth = renderTarget.Width * scale;
            newHeight = renderTarget.Height * scale;

            posX = (screenDimensions.Width - newWidth) / 2.0f;
            posY = (screenDimensions.Height - newHeight) / 2.0f;

            adjustedScreen = new(
                (int)posX,
                (int)posY,
                (int)newWidth,
                (int)newHeight
            );

            return adjustedScreen;
        }
    }
}

