using StardustSandbox.Core.Localization;

namespace StardustSandbox.Core.Constants
{
    public static class SLocalizationConstants
    {
        public static SGameCulture DefaultCulture => Cultures[0];

        public static SGameCulture[] Cultures => [
            new("en", "US"),
            new("pt", "BR"),
        ];
    }
}
