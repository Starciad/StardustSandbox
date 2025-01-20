using MessagePack;

using System;

namespace StardustSandbox.Core.IO.Files.Saving.World.Environment
{
    [MessagePackObject]
    public sealed class SSaveFileWorldTime
    {
        [Key(0)] public TimeSpan CurrentTime { get; set; } = TimeSpan.Zero;
        [Key(1)] public bool IsFrozen { get; set; } = false;
    }
}
