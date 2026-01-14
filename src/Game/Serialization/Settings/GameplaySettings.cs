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

using StardustSandbox.Colors.Palettes;

using System;
using System.Xml.Serialization;

namespace StardustSandbox.Serialization.Settings
{
    [Serializable]
    [XmlRoot("GameplaySettings")]
    public readonly struct GameplaySettings : ISettingsModule
    {
        [XmlElement("ShowPreviewArea", typeof(bool))]
        public readonly bool ShowPreviewArea { get; init; }

        [XmlElement("PreviewAreaColorR", typeof(byte))]
        public readonly byte PreviewAreaColorR { get; init; }

        [XmlElement("PreviewAreaColorG", typeof(byte))]
        public readonly byte PreviewAreaColorG { get; init; }

        [XmlElement("PreviewAreaColorB", typeof(byte))]
        public readonly byte PreviewAreaColorB { get; init; }

        [XmlElement("PreviewAreaColorA", typeof(byte))]
        public readonly byte PreviewAreaColorA { get; init; }

        [XmlElement("ShowGrid", typeof(bool))]
        public readonly bool ShowGrid { get; init; }

        [XmlElement("GridOpacity", typeof(byte))]
        public readonly byte GridOpacity { get; init; }

        [XmlElement("ShowTemperatureColorVariations", typeof(bool))]
        public readonly bool ShowTemperatureColorVariations { get; init; }

        [XmlIgnore]
        public Color PreviewAreaColor
        {
            readonly get => new(this.PreviewAreaColorR, this.PreviewAreaColorG, this.PreviewAreaColorB, this.PreviewAreaColorA);

            init
            {
                this.PreviewAreaColorR = value.R;
                this.PreviewAreaColorG = value.G;
                this.PreviewAreaColorB = value.B;
                this.PreviewAreaColorA = value.A;
            }
        }

        public GameplaySettings()
        {
            this.ShowPreviewArea = true;
            this.PreviewAreaColor = AAP64ColorPalette.White;
            this.PreviewAreaColorA = 80;
            this.ShowGrid = true;
            this.GridOpacity = 96;
            this.ShowTemperatureColorVariations = true;
        }
    }
}

