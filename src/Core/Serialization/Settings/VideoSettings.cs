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
using StardustSandbox.Core.Interfaces.Serialization;

using System;
using System.Xml.Serialization;

namespace StardustSandbox.Core.Serialization.Settings
{
    [Serializable]
    [XmlRoot("VideoSettings")]
    public sealed class VideoSettings : ISettingsModule
    {
        [XmlElement("Framerate", typeof(float))]
        public float Framerate { get; set; }

        [XmlElement("Width", typeof(int))]
        public int Width { get; set; }

        [XmlElement("Height", typeof(int))]
        public int Height { get; set; }

        [XmlElement("FullScreen", typeof(bool))]
        public bool FullScreen { get; set; }

        [XmlElement("VSync", typeof(bool))]
        public bool VSync { get; set; }

        [XmlElement("Borderless", typeof(bool))]
        public bool Borderless { get; set; }

        [XmlIgnore]
        public Point Resolution
        {
            get => new(this.Width, this.Height);

            set
            {
                this.Width = value.X;
                this.Height = value.Y;
            }
        }

        public VideoSettings()
        {
            this.Framerate = ScreenConstants.FRAMERATE;
            this.Width = 0;
            this.Height = 0;
            this.FullScreen = false;
            this.VSync = false;
            this.Borderless = false;
        }

        public void UpdateResolution(GraphicsDevice graphicsDevice)
        {
            Point monitorResolution = new(
                graphicsDevice.Adapter.CurrentDisplayMode.Width,
                graphicsDevice.Adapter.CurrentDisplayMode.Height
            );

            Point autoResolution = GetAutoResolution(monitorResolution);

            this.Width = autoResolution.X;
            this.Height = autoResolution.Y;
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

