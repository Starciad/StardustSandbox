using MessagePack;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Serialization.Saving.Data
{
    [MessagePackObject]
    public sealed class ManifestData
    {
        [Key("FormatVersion")]
        public Version FormatVersion { get; set; }

        [Key("ComponentVersions")]
        public Dictionary<string, Version> ComponentVersions { get; set; }

        [Key("GameVersion")]
        public Version GameVersion { get; set; }

        [Key("CreationTimestamp")]
        public DateTime CreationTimestamp { get; set; }
    }
}
