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

using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Interfaces.Serialization.Morph;

using System;
using System.Xml.Serialization;

namespace StardustSandbox.Core.Serialization.Common.Settings.Data.V1
{
    [Serializable]
    [XmlRoot("CursorData")]
    public sealed class CursorData : IData
    {
        [XmlElement("ForegroundColorR", typeof(byte))]
        public byte ForegroundColorR { get; set; }

        [XmlElement("ForegroundColorG", typeof(byte))]
        public byte ForegroundColorG { get; set; }

        [XmlElement("ForegroundColorB", typeof(byte))]
        public byte ForegroundColorB { get; set; }

        [XmlElement("ForegroundColorA", typeof(byte))]
        public byte ForegroundColorA { get; set; }

        [XmlElement("BackgroundColorR", typeof(byte))]
        public byte BackgroundColorR { get; set; }

        [XmlElement("BackgroundColorG", typeof(byte))]
        public byte BackgroundColorG { get; set; }

        [XmlElement("BackgroundColorB", typeof(byte))]
        public byte BackgroundColorB { get; set; }

        [XmlElement("BackgroundColorA", typeof(byte))]
        public byte BackgroundColorA { get; set; }

        [XmlElement("Scale", typeof(float))]
        public float Scale { get; set; }
    }
}
