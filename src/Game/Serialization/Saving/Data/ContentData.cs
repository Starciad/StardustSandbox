using MessagePack;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Serialization.Saving.Data
{
    [Serializable]
    [MessagePackObject]
    public sealed class ContentData
    {
        [Key("Slots")]
        public IEnumerable<SlotData> Slots { get; init; }

        [Key("Actors")]
        public IEnumerable<ActorData> Actors { get; init; }
    }
}
