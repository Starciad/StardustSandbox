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

        [Key("Content")]
        public IReadOnlyDictionary<string, object> Content { get; init; }
    }
}
