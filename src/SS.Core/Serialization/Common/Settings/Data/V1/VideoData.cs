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

using StardustSandbox.Core.Interfaces.Serialization.Morph;

using System;
using System.Xml.Serialization;

namespace StardustSandbox.Core.Serialization.Common.Settings.Data.V1
{
    [Serializable]
    [XmlRoot("VideoData")]
    public sealed class VideoData : IData
    {
        [XmlElement("Borderless", typeof(bool))]
        public bool Borderless { get; set; }

        [XmlElement("Framerate", typeof(float))]
        public float Framerate { get; set; }

        [XmlElement("FullScreen", typeof(bool))]
        public bool FullScreen { get; set; }

        [XmlElement("Height", typeof(int))]
        public int Height { get; set; }

        [XmlElement("VSync", typeof(bool))]
        public bool VSync { get; set; }

        [XmlElement("Width", typeof(int))]
        public int Width { get; set; }
    }
}

