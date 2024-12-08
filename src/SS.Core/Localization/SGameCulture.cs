using System.Globalization;

namespace StardustSandbox.Core.Localization
{
    public class SGameCulture
    {
        public CultureInfo CultureInfo => cultureInfo;
        public string Name => string.Concat(this.Language, '-', this.Region);
        public string Language => language;
        public string Region => region;

        private readonly CultureInfo cultureInfo;
        private readonly string language;
        private readonly string region;

        public SGameCulture(string language, string region)
        {
            this.language = language;
            this.region = region;
            this.cultureInfo = new(this.Name);
        }
    }
}
