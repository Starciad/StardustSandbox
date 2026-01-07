using MessagePack;

using System;

namespace StardustSandbox.Serialization.Saving.Data
{
    [Serializable]
    [MessagePackObject]
    public sealed class ManifestData
    {
        [Key("GameVersion")]
        public Version GameVersion { get; init; }

        [Key("CreationTimestamp")]
        public DateTime CreationTimestamp { get; init; }
    }
}
