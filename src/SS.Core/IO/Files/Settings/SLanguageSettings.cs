using MessagePack;

namespace StardustSandbox.Core.IO.Files.Settings
{
    [MessagePackObject]
    public sealed class SLanguageSettings : SSettings
    {
        [Key(0)]
        public string Language { get; set; }

        [Key(1)]
        public string Region { get; set; }

        public SLanguageSettings()
        {
            this.Language = "en";
            this.Region = "US";
        }
    }
}
