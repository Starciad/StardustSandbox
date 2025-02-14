using MessagePack;

using System;

namespace StardustSandbox.Core.IO.Files.Saving.Header
{
    [MessagePackObject]
    public sealed class SSaveFileInformation
    {
        [Key(0)]
        public Version SaveVersion { get; set; } = new();

        [Key(1)]
        public Version GameVersion { get; set; } = new();

        [Key(2)]
        public DateTime CreationTimestamp { get; set; } = DateTime.Now;

        [Key(3)]
        public DateTime LastUpdateTimestamp { get; set; } = DateTime.Now;
    }
}
