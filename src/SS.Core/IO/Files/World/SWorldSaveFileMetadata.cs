using MessagePack;

using System;

namespace StardustSandbox.Core.IO.Files.World
{
    [MessagePackObject]
    public sealed class SWorldSaveFileMetadata
    {
        [Key(0)]
        public string Name { get; set; }

        [Key(1)]
        public string Description { get; set; }

        [Key(2)]
        public Version Version { get; set; }

        [Key(3)]
        public DateTime CreationTimestamp { get; set; }

        [Key(4)]
        public DateTime LastUpdateTimestamp { get; set; }
    }
}
