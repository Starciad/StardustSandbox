using MessagePack;

using System;

namespace StardustSandbox.Serialization.Saving.Data
{
    [Serializable]
    [MessagePackObject]
    public sealed class ContentData
    {
        [Key("Slots")]
        public SlotData[] Slots { get; init; }

        [Key("Actors")]
        public byte[][] Actors { get; init; }
    }
}
