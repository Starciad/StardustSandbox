using MessagePack;

using System;

namespace StardustSandbox.Serialization.Saving.Data
{
    [Serializable]
    [MessagePackObject]
    public sealed class Metadata
    {
        [Key("Name")]
        public string Name { get; init; }

        [Key("Description")]
        public string Description { get; init; }
    }
}
