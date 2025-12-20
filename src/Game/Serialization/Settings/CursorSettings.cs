using Microsoft.Xna.Framework;

using StardustSandbox.Colors.Palettes;

using System;
using System.Xml.Serialization;

namespace StardustSandbox.Serialization.Settings
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