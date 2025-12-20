using StardustSandbox.Constants;
using StardustSandbox.Localization;

using System;
using System.Globalization;
using System.Xml.Serialization;

namespace StardustSandbox.Serialization.Settings
{
    [Serializable]
    [XmlRoot("GeneralSettings")]
    public readonly struct GeneralSettings : ISettingsModule
    {
        [XmlElement("Language", typeof(string))]
        public readonly string Language { get; init; }

        [XmlElement("Region", typeof(string))]
        public readonly string Region { get; init; }

        [XmlIgnore]
        public readonly string Name => string.Concat(this.Language, '-', this.Region);

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

        public readonly GameCulture GetGameCulture()
        {
            return LocalizationConstants.GetGameCulture(this.Name) ?? LocalizationConstants.DEFAULT_GAME_CULTURE;
        }

        private static bool TryGetAvailableGameCulture(out GameCulture value)
        {
            value = LocalizationConstants.GetGameCulture(CultureInfo.CurrentCulture.Name);

            return value != null;
        }
    }
}
