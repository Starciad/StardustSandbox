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
using StardustSandbox.Core.Interfaces.Serialization.Morph;

using System;
using System.Xml.Serialization;

namespace StardustSandbox.Core.Serialization.Common.Settings.StorageModels
{
    public sealed class VideoStorageModel : IStorageModel
    {
        public float Framerate { get; set; }
        public Point Resolution { get; set; }
        public bool FullScreen { get; set; }
        public bool VSync { get; set; }
        public bool Borderless { get; set; }

        public VideoStorageModel()
        {
            this.Framerate = 60.0f;
            this.Resolution = Point.Zero;
            this.FullScreen = false;
            this.VSync = true;
            this.Borderless = false;
        }

        public void UpdateResolution(GraphicsDevice graphicsDevice)
        {
            Point monitorResolution = new(
                graphicsDevice.Adapter.CurrentDisplayMode.Width,
                graphicsDevice.Adapter.CurrentDisplayMode.Height
            );

            Point autoResolution = GetAutoResolution(monitorResolution);

            this.Resolution = autoResolution;
        }

        private static Point GetAutoResolution(Point monitorResolution)
        {
            for (int i = ScreenConstants.RESOLUTIONS.Length - 1; i >= 0; i--)
            {
                Point resolution = ScreenConstants.RESOLUTIONS[i];

                if (resolution.X <= monitorResolution.X && resolution.Y <= monitorResolution.Y)
                {
                    return resolution;
                }
            }

            return ScreenConstants.RESOLUTIONS[0];
        }
    }
}

