using StardustSandbox.Constants;
using StardustSandbox.LocalizationSystem;

using System.Globalization;
using System.Xml.Serialization;

namespace StardustSandbox.IO.Settings
{
    [XmlRoot("GeneralSettings")]
    public sealed class GeneralSettings : SettingsModule
    {
        [XmlElement("Language", typeof(string))]
        public string Language { get; set; }

        [XmlElement("Region", typeof(string))]
        public string Region { get; set; }

        [XmlIgnore]
        public GameCulture GameCulture => LocalizationConstants.GetGameCulture(this.Name);

        [XmlIgnore]
        public string Name => string.Concat(this.Language, '-', this.Region);

        public GeneralSettings()
        {
            GameCulture gameCulture = LocalizationConstants.DEFAULT_GAME_CULTURE;

            if (TryGetAvailableGameCulture(out GameCulture value))
            {
                gameCulture = value;
            }

            this.Language = gameCulture.Language;
            this.Region = gameCulture.Region;
        }

        public static bool TryGetAvailableGameCulture(out GameCulture value)
        {
            value = LocalizationConstants.GetGameCulture(CultureInfo.CurrentCulture.Name);

            return value != null;
        }
    }
}
