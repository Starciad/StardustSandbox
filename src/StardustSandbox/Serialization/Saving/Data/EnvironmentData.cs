using MessagePack;

namespace StardustSandbox.Serialization.Saving.Data
{
    [MessagePackObject]
    public sealed class EnvironmentData
    {
        [Key("Time")]
        public TimeData Time { get; set; }
    }
}
