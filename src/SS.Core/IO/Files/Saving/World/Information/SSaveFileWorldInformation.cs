using MessagePack;

using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.Core.IO.Files.Saving.World.Information
{
    [MessagePackObject]
    public sealed class SSaveFileWorldInformation
    {
        [IgnoreMember] public SSize2 Size => new(this.Width, this.Height);
        [Key(0)] public int Width { get; set; } = 0;
        [Key(1)] public int Height { get; set; } = 0;
    }
}
