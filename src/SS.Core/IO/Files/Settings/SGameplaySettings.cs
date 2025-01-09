using Microsoft.Xna.Framework;

using StardustSandbox.Core.Colors;

using System.Xml.Serialization;

namespace StardustSandbox.Core.IO.Files.Settings
{
    [XmlRoot("GameplaySettings")]
    public sealed class SGameplaySettings : SSettings
    {
        [XmlElement("PreviewAreaColor", typeof(Color))]
        public Color PreviewAreaColor { get; set; }

        [XmlElement("PreviewAreaOpacity", typeof(float))]
        public float PreviewAreaOpacity { get; set; }

        public SGameplaySettings()
        {
            this.PreviewAreaColor = SColorPalette.White;
            this.PreviewAreaOpacity = 25f;
        }
    }
}
