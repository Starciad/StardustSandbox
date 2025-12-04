using MessagePack;

namespace StardustSandbox.Serialization.Saving.Data
{
    [MessagePackObject]
    public sealed class ContentData
    {
        [Key("Slots")]
        public SlotData[] Slots { get; set; }
    }
}
