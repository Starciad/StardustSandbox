using MessagePack;

using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.Core.IO.Files.World.Data
{
    [MessagePackObject]
    public sealed class SWorldData
    {
        [IgnoreMember] public SSize2 Size => new(this.Width, this.Height);
        [Key(0)] public int Width { get; set; }
        [Key(1)] public int Height { get; set; }
        [Key(2)] public SWorldSlotData[] Slots { get; set; }
    }
}
