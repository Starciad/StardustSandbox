using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Localization;

using System.Globalization;
using System.Xml.Serialization;

namespace StardustSandbox.Core.IO.Files.Settings
{
    [XmlRoot("GeneralSettings")]
    public sealed class SGeneralSettings : SSettings
    {
        [XmlIgnore]
        public SGameCulture GameCulture => SLocalizationConstants.GetGameCulture(this.Name);

        [XmlIgnore]
        public string Name => string.Concat(this.Language, '-', this.Region);

        [XmlElement("Language", typeof(string))]
        public string Language { get; set; }

        [XmlElement("Region", typeof(string))]
        public string Region { get; set; }

        public SGeneralSettings()
        {
            SGameCulture gameCulture = SLocalizationConstants.DEFAULT_GAME_CULTURE;

            if (TryGetAvailableGameCulture(out SGameCulture value))
            {
                gameCulture = value;
            }

            this.Language = gameCulture.Language;
            this.Region = gameCulture.Region;
        }

        public static bool TryGetAvailableGameCulture(out SGameCulture value)
        {
            value = SLocalizationConstants.GetGameCulture(CultureInfo.CurrentCulture.Name);

            return value != null;
        }
    }
}
