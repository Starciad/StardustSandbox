namespace StardustSandbox.Core.Localization
{
    public sealed class SGameCulture(string language, string region)
    {
        public string Name => string.Concat(this.Language, '-', this.Region);
        public string Language => language.ToLowerInvariant();
        public string Region => region.ToUpperInvariant();
    }
}
