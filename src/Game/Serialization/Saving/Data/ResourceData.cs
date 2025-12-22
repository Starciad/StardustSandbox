using MessagePack;

using StardustSandbox.Enums.Elements;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Serialization.Saving.Data
{
    [Serializable]
    [MessagePackObject]
    public sealed class ResourceData
    {
        [Key("Elements")]
        public IEnumerable<ElementIndex> Elements { get; init; }
    }
}
