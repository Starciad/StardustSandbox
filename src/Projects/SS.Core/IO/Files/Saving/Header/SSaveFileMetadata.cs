using MessagePack;

namespace StardustSandbox.Core.IO.Files.Saving.Header
{
    [MessagePackObject]
    public sealed class SSaveFileMetadata
    {
        [Key(0)] public string Filename { get; set; } = string.Empty;
        [Key(1)] public string Identifier { get; set; } = string.Empty;
        [Key(2)] public string Name { get; set; } = string.Empty;
        [Key(3)] public string Description { get; set; } = string.Empty;
    }
}
