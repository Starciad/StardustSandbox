using MessagePack;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Localization;

using System.Globalization;

namespace StardustSandbox.Core.IO.Files.Settings
{
    [MessagePackObject]
    public sealed class SLanguageSettings : SSettings
    {
        [IgnoreMember]
        public SGameCulture GameCulture { get; private set; }

        [Key(0)]
        public string Language { get; set; }

        [Key(1)]
        public string Region { get; set; }

        public SLanguageSettings()
        {
            SGameCulture gameCulture = SLocalizationConstants.DEFAULT_GAME_CULTURE;

            if (TryGetAvailableGameCulture(out SGameCulture value))
            {
                gameCulture = value;
            }

            SetGameCulture(gameCulture);
        }

        public static bool TryGetAvailableGameCulture(out SGameCulture value)
        {
            value = SLocalizationConstants.GetGameCulture(CultureInfo.CurrentCulture.Name);

            return value != null;
        }

        public void SetGameCulture(SGameCulture gameCulture)
        {
            this.GameCulture = gameCulture;
            this.Language = gameCulture.Language;
            this.Region = gameCulture.Region;
        }
    }
}
