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

using System;
using System.Xml.Serialization;

namespace StardustSandbox.Core.Serialization.Settings
{
    [Serializable]
    [XmlRoot("CursorSettings")]
    public readonly struct CursorSettings : ISettingsModule
    {
        [XmlElement("ColorR", typeof(byte))]
        public readonly byte ColorR { get; init; }

        [XmlElement("ColorG", typeof(byte))]
        public readonly byte ColorG { get; init; }

        [XmlElement("ColorB", typeof(byte))]
        public readonly byte ColorB { get; init; }

        [XmlElement("BackgroundColorR", typeof(byte))]
        public readonly byte BackgroundColorR { get; init; }

        [XmlElement("BackgroundColorG", typeof(byte))]
        public readonly byte BackgroundColorG { get; init; }

        [XmlElement("BackgroundColorB", typeof(byte))]
        public readonly byte BackgroundColorB { get; init; }

        [XmlElement("Scale", typeof(float))]
        public readonly float Scale { get; init; }

        [XmlElement("Alpha", typeof(byte))]
        public readonly byte Alpha { get; init; }

        [XmlIgnore]
        public Color Color
        {
            readonly get => new(this.ColorR, this.ColorG, this.ColorB, this.Alpha);

            init
            {
                this.ColorR = value.R;
                this.ColorG = value.G;
                this.ColorB = value.B;
            }
        }

        [XmlIgnore]
        public Color BackgroundColor
        {
            readonly get => new(this.BackgroundColorR, this.BackgroundColorG, this.BackgroundColorB, this.Alpha);

            init
            {
                this.BackgroundColorR = value.R;
                this.BackgroundColorG = value.G;
                this.BackgroundColorB = value.B;
            }
        }
        public CursorSettings()
        {
            this.Color = AAP64ColorPalette.White;
            this.BackgroundColor = AAP64ColorPalette.DarkRed;
            this.Scale = 1.0f;
            this.Alpha = 255;
        }
    }
}
