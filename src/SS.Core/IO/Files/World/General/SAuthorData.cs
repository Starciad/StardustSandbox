using MessagePack;

namespace StardustSandbox.Core.IO.Files.World.General
{
    [MessagePackObject]
    public sealed class SAuthorData
    {
        [Key(0)]
        public string Name { get; set; }

        [Key(1)]
        public string Id { get; set; }
    }
}
