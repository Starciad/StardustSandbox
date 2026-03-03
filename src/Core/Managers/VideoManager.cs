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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Serialization.Settings;

namespace StardustSandbox.Core.Managers
{
    internal sealed class VideoManager
    {
        internal GraphicsDeviceManager GraphicsDeviceManager => this.graphicsDeviceManager;
        internal GraphicsDevice GraphicsDevice => this.graphicsDeviceManager.GraphicsDevice;

        private readonly GraphicsDeviceManager graphicsDeviceManager;
        private readonly GameWindow gameWindow;

        internal VideoManager(GraphicsDeviceManager graphicsDeviceManager, GameWindow gameWindow)
        {
            this.graphicsDeviceManager = graphicsDeviceManager;
            this.gameWindow = gameWindow;
        }

        internal void ApplySettings(VideoSettings videoSettings)
        {
            SetResolution(videoSettings.Width, videoSettings.Height);
            SetFullScreen(videoSettings.FullScreen);
            SetVSync(videoSettings.VSync);
            SetBorderless(videoSettings.Borderless);
        }

        internal void SetResolution(int width, int height)
        {
            Point minSize = ScreenConstants.RESOLUTIONS[0];
            Point newSize = new(this.gameWindow.ClientBounds.Width, this.gameWindow.ClientBounds.Height);

            if (newSize.X < minSize.X)
            {
                newSize.X = minSize.X;
            }

            if (newSize.Y < minSize.Y)
            {
                newSize.Y = minSize.Y;
            }

            this.graphicsDeviceManager.PreferredBackBufferWidth = width;
            this.graphicsDeviceManager.PreferredBackBufferHeight = height;
            this.graphicsDeviceManager.ApplyChanges();
        }

        internal void SetResolution(Point resolution)
        {
            SetResolution(resolution.X, resolution.Y);
        }

        internal void SetFullScreen(bool value)
        {
            this.graphicsDeviceManager.IsFullScreen = value;
            this.graphicsDeviceManager.ApplyChanges();
        }

        internal void SetVSync(bool value)
        {
            this.graphicsDeviceManager.SynchronizeWithVerticalRetrace = value;
            this.graphicsDeviceManager.ApplyChanges();
        }

        internal void SetBorderless(bool value)
        {
            this.gameWindow.IsBorderless = value;
        }
    }
}
