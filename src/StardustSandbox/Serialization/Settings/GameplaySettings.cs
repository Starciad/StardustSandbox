using Microsoft.Xna.Framework;

using StardustSandbox.Colors.Palettes;

using System.Xml.Serialization;

namespace StardustSandbox.Serialization.Settings
{
    [XmlRoot("GameplaySettings")]
    public sealed class GameplaySettings : SettingsModule
    {
        [XmlIgnore]
        public Color PreviewAreaColor
        {
            get => new(this.PreviewAreaColorR, this.PreviewAreaColorG, this.PreviewAreaColorB, this.PreviewAreaColorA);

            set
            {
                this.PreviewAreaColorR = value.R;
                this.PreviewAreaColorG = value.G;
                this.PreviewAreaColorB = value.B;
                this.PreviewAreaColorA = value.A;
            }
        }

        [XmlElement("PreviewAreaColorR", typeof(byte))]
        public byte PreviewAreaColorR { get; set; }

        [XmlElement("PreviewAreaColorG", typeof(byte))]
        public byte PreviewAreaColorG { get; set; }

        [XmlElement("PreviewAreaColorB", typeof(byte))]
        public byte PreviewAreaColorB { get; set; }

        [XmlElement("PreviewAreaColorA", typeof(byte))]
        public byte PreviewAreaColorA { get; set; }

        public GameplaySettings()
        {
            this.PreviewAreaColor = AAP64ColorPalette.White;
            this.PreviewAreaColorA = 25;
        }
    }
}
