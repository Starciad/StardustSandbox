using System.Globalization;

namespace StardustSandbox.Localization
{
    public class GameCulture
    {
        public CultureInfo CultureInfo => this.cultureInfo;
        public string Name => string.Concat(this.Language, '-', this.Region);
        public string Language => this.language;
        public string Region => this.region;

        private readonly CultureInfo cultureInfo;
        private readonly string language;
        private readonly string region;

        public GameCulture(string language, string region)
        {
            this.language = language;
            this.region = region;
            this.cultureInfo = new(this.Name);
        }
    }
}
