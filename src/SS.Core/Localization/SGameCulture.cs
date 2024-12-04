namespace StardustSandbox.Core.Localization
{
    public class SGameCulture(string language, string region)
    {
        public string Name => string.Concat(this.Language, '-', this.Region);
        public string Language => language;
        public string Region => region;
    }
}
