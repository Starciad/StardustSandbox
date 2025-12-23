using StardustSandbox.Localization;

using System;

namespace StardustSandbox.Constants
{
    internal static class LocalizationConstants
    {
        internal static GameCulture DEFAULT_GAME_CULTURE => gameCultures[0];
        internal static GameCulture[] AVAILABLE_GAME_CULTURES => gameCultures;

        private static readonly GameCulture[] gameCultures =
        [
            new("en", "US"),
            new("pt", "BR"),
            new("es", "ES"),
            new("fr", "FR"),
            new("de", "DE"),
        ];

        internal static GameCulture GetGameCultureFromNativeName(string nativeName)
        {
            return Array.Find(gameCultures, x => x.CultureInfo.NativeName.Equals(nativeName, StringComparison.OrdinalIgnoreCase)) ?? DEFAULT_GAME_CULTURE;
        }

        internal static GameCulture GetGameCulture(string name)
        {
            return Array.Find(gameCultures, x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
