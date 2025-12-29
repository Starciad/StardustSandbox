using MessagePack;

using StardustSandbox.Enums.Actors;

using System.Collections.Generic;

namespace StardustSandbox.Serialization.Saving.Data
{
    [MessagePackObject]
    public sealed class ActorData
    {
        [Key("Index")]
        public required ActorIndex Index { get; init; }

        [Key("Data")]
        public IReadOnlyDictionary<string, object> Data { get; init; }
    }
}
