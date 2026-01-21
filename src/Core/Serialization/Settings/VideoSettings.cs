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

using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Mathematics.Primitives;

using System;
using System.Xml.Serialization;

namespace StardustSandbox.Core.Serialization.Settings
{
    [Serializable]
    [XmlRoot("VideoSettings")]
    public readonly struct VideoSettings : ISettingsModule
    {
        [XmlElement("Framerate", typeof(float))]
        public readonly float Framerate { get; init; }

        [XmlElement("Width", typeof(int))]
        public readonly int Width { get; init; }

        [XmlElement("Height", typeof(int))]
        public readonly int Height { get; init; }

        [XmlElement("FullScreen", typeof(bool))]
        public readonly bool FullScreen { get; init; }

        [XmlElement("VSync", typeof(bool))]
        public readonly bool VSync { get; init; }

        [XmlElement("Borderless", typeof(bool))]
        public readonly bool Borderless { get; init; }

        [XmlIgnore]
        public Resolution Resolution
        {
            readonly get => new(this.Width, this.Height);

            init
            {
                this.Width = value.Width;
                this.Height = value.Height;
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

        public VideoSettings UpdateResolution(GraphicsDevice graphicsDevice)
        {
            Resolution monitorResolution = new(
                graphicsDevice.Adapter.CurrentDisplayMode.Width,
                graphicsDevice.Adapter.CurrentDisplayMode.Height
            );

            Resolution autoResolution = GetAutoResolution(monitorResolution);

            return new()
            {
                Framerate = this.Framerate,
                Width = autoResolution.Width,
                Height = autoResolution.Height,
                FullScreen = this.FullScreen,
                VSync = this.VSync,
                Borderless = this.Borderless
            };
        }

        private static Resolution GetAutoResolution(Resolution monitorResolution)
        {
            for (int i = ScreenConstants.RESOLUTIONS.Length - 1; i >= 0; i--)
            {
                Resolution resolution = ScreenConstants.RESOLUTIONS[i];

                if (resolution.Width <= monitorResolution.Width && resolution.Height <= monitorResolution.Height)
                {
                    return resolution;
                }
            }

            return ScreenConstants.RESOLUTIONS[0];
        }
    }
}

