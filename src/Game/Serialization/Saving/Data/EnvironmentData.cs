using MessagePack;

using System;

namespace StardustSandbox.Serialization.Saving.Data
{
    [Serializable]
    [MessagePackObject]
    public sealed class EnvironmentData
    {
        [Key("CurrentTime")]
        public TimeSpan CurrentTime { get; init; }

        [Key("IsFrozen")]
        public bool IsFrozen { get; init; }
    }
}
