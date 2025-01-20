using MessagePack;

namespace StardustSandbox.Core.IO.Files.Saving.World.Information
{
    [MessagePackObject]
    public sealed class SSaveFileResource(uint index, string value)
    {
        [Key(0)] public uint Index => index;
        [Key(1)] public string Value => value;
    }
}
