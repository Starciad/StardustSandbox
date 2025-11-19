using MessagePack;

using Microsoft.Xna.Framework;

namespace StardustSandbox.IO.Saving.World.Information
{
    [MessagePackObject]
    public sealed class SaveFileWorldInformation
    {
        [Key(0)] public int Width { get; set; } = 0;
        [Key(1)] public int Height { get; set; } = 0;
        [IgnoreMember] public Point Size => new(this.Width, this.Height);
    }
}
