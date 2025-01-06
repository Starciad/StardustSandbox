using Microsoft.Xna.Framework;

using System.Xml.Serialization;

namespace StardustSandbox.Core.IO.Files.Settings
{
    [XmlRoot("CursorSettings")]
    public sealed class SCursorSettings : SSettings
    {
        [XmlIgnore]
        public Color Color
        {
            get => new(this.ColorR, this.ColorG, this.ColorB, this.ColorA);

            set
            {
                this.ColorR = value.R;
                this.ColorG = value.G;
                this.ColorB = value.B;
                this.ColorA = value.A;
            }
        }

        [XmlIgnore]
        public Color BackgroundColor
        {
            get => new(this.BackgroundColorR, this.BackgroundColorG, this.BackgroundColorB, this.BackgroundColorA);

            set
            {
                this.BackgroundColorR = value.R;
                this.BackgroundColorG = value.G;
                this.BackgroundColorB = value.B;
                this.BackgroundColorA = value.A;
            }
        }

        [XmlElement("Scale", typeof(float))] public float Scale { get; set; }
        [XmlElement("ColorR", typeof(byte))] public byte ColorR { get; set; }
        [XmlElement("ColorG", typeof(byte))] public byte ColorG { get; set; }
        [XmlElement("ColorB", typeof(byte))] public byte ColorB { get; set; }
        [XmlElement("ColorA", typeof(byte))] public byte ColorA { get; set; }
        [XmlElement("BackgroundR", typeof(byte))] public byte BackgroundColorR { get; set; }
        [XmlElement("BackgroundG", typeof(byte))] public byte BackgroundColorG { get; set; }
        [XmlElement("BackgroundB", typeof(byte))] public byte BackgroundColorB { get; set; }
        [XmlElement("BackgroundA", typeof(byte))] public byte BackgroundColorA { get; set; }

        public SCursorSettings()
        {
            this.Scale = 1f;
            this.Color = Color.White;
            this.BackgroundColor = Color.Red;
        }
    }
}