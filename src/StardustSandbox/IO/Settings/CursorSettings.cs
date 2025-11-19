using Microsoft.Xna.Framework;

using StardustSandbox.Colors.Palettes;

using System.Xml.Serialization;

namespace StardustSandbox.IO.Settings
{
    [XmlRoot("CursorSettings")]
    public sealed class CursorSettings : SettingsModule
    {
        [XmlIgnore]
        public Color Color
        {
            get => new(this.ColorR, this.ColorG, this.ColorB, this.Alpha);

            set
            {
                this.ColorR = value.R;
                this.ColorG = value.G;
                this.ColorB = value.B;
            }
        }

        [XmlIgnore]
        public Color BackgroundColor
        {
            get => new(this.BackgroundColorR, this.BackgroundColorG, this.BackgroundColorB, this.Alpha);

            set
            {
                this.BackgroundColorR = value.R;
                this.BackgroundColorG = value.G;
                this.BackgroundColorB = value.B;
            }
        }

        [XmlElement("ColorR", typeof(byte))]
        public byte ColorR { get; set; }

        [XmlElement("ColorG", typeof(byte))]
        public byte ColorG { get; set; }

        [XmlElement("ColorB", typeof(byte))]
        public byte ColorB { get; set; }

        [XmlElement("BackgroundColorR", typeof(byte))]
        public byte BackgroundColorR { get; set; }

        [XmlElement("BackgroundColorG", typeof(byte))]
        public byte BackgroundColorG { get; set; }

        [XmlElement("BackgroundColorB", typeof(byte))]
        public byte BackgroundColorB { get; set; }

        [XmlElement("Scale", typeof(float))]
        public float Scale { get; set; }

        [XmlElement("Alpha", typeof(byte))]
        public byte Alpha { get; set; }

        public CursorSettings()
        {
            this.Color = AAP64ColorPalette.White;
            this.BackgroundColor = AAP64ColorPalette.DarkRed;
            this.Scale = 1f;
            this.Alpha = 255;
        }
    }
}