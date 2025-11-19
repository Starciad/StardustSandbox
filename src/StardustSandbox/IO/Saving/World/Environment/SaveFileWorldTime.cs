using MessagePack;

using System;

namespace StardustSandbox.IO.Saving.World.Environment
{
    [MessagePackObject]
    public sealed class SaveFileWorldTime
    {
        [Key(0)] public TimeSpan CurrentTime { get; set; } = TimeSpan.Zero;
        [Key(1)] public bool IsFrozen { get; set; } = false;
    }
}
