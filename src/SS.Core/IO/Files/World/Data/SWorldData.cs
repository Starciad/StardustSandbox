using MessagePack;

namespace StardustSandbox.Core.IO.Files.World.Data
{
    [MessagePackObject]
    public sealed class SWorldData
    {
        [Key(0)] public ushort Width { get; set; }
        [Key(1)] public ushort Height { get; set; }
        [Key(2)] public SSlotData[] Slots { get; set; }
    }

    [MessagePackObject]
    public sealed class SSlotData
    {
        [Key(0)] public byte ElementId { get; set; }
        [Key(1)] public bool IsEmpty { get; set; }
        [Key(2)] public short Temperature { get; set; }
        [Key(3)] public bool FreeFalling { get; set; }
        [Key(4)] public byte ColorR { get; set; }
        [Key(5)] public byte ColorG { get; set; }
        [Key(6)] public byte ColorB { get; set; }
        [Key(7)] public byte ColorA { get; set; }
    }
}
