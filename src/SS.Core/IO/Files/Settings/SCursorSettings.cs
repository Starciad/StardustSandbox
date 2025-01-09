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

        [XmlElement("Opacity", typeof(float))]
        public float Opacity { get; set; }

        public SCursorSettings()
        {
            this.Scale = 1f;
            this.Color = SColorPalette.White;
            this.BackgroundColor = SColorPalette.DarkRed;
        }
    }
}