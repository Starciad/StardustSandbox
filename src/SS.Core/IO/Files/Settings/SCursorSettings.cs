using Microsoft.Xna.Framework;

using StardustSandbox.Core.Colors;

using System.Xml.Serialization;

namespace StardustSandbox.Core.IO.Files.Settings
{
    [XmlRoot("CursorSettings")]
    public sealed class SCursorSettings : SSettings
    {
        [XmlElement("Color", typeof(Color))]
        public Color Color { get; set; }

        [XmlElement("BackgroundColor", typeof(Color))]
        public Color BackgroundColor { get; set; }

        [XmlElement("Scale", typeof(float))]
        public float Scale { get; set; }

        [XmlElement("Opacity", typeof(byte))]
        public byte Opacity { get; set; }

        public SCursorSettings()
        {
            this.Color = SColorPalette.White;
            this.BackgroundColor = SColorPalette.DarkRed;
            this.Scale = 1f;
            this.Opacity = 255;
        }
    }
}