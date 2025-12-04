using MessagePack;

using System;

namespace StardustSandbox.Serialization.Saving.Data
{
    [MessagePackObject]
    public sealed class TimeData
    {
        [Key("CurrentTime")]
        public TimeSpan CurrentTime { get; set; }

        [Key("IsFrozen")]
        public bool IsFrozen { get; set; }
    }
}
