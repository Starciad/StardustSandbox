using StardustSandbox.Core.Localization;

using System;

namespace StardustSandbox.Core.Constants
{
    public static class SLocalizationConstants
    {
        public static SGameCulture DEFAULT_GAME_CULTURE => AVAILABLE_GAME_CULTURES[0];
        
        public static SGameCulture[] AVAILABLE_GAME_CULTURES => [
            new("en", "US"),
            new("pt", "BR"),
        ];

        public static SGameCulture GetGameCulture(string name)
        {
            return Array.Find(AVAILABLE_GAME_CULTURES, x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
