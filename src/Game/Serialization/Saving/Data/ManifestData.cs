using MessagePack;

using System;

namespace StardustSandbox.Serialization.Saving.Data
{
    [Serializable]
    [MessagePackObject]
    public sealed class ManifestData
    {
        [Key("FormatVersion")]
        public byte FormatVersion { get; init; }

        [Key("GameVersion")]
        public Version Version { get; init; }

        [Key("CreationTimestamp")]
        public DateTime CreationTimestamp { get; init; }
    }
}
