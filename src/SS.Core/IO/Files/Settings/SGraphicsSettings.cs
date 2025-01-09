using System.Xml.Serialization;

namespace StardustSandbox.Core.IO.Files.Settings
{
    [XmlRoot("GraphicsSettings")]
    public sealed class SGraphicsSettings : SSettings
    {
        [XmlElement("Lighting")]
        public bool Lighting { get; set; }

        public SGraphicsSettings()
        {
            this.Lighting = true;
        }
    }
}
