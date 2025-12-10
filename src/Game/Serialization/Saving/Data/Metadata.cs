using MessagePack;

namespace StardustSandbox.Serialization.Saving.Data
{
    [MessagePackObject]
    public sealed class Metadata
    {
        [IgnoreMember]
        public string Filename { get; set; }

        [Key("Identifier")]
        public string Identifier { get; set; }

        [Key("Name")]
        public string Name { get; set; }

        [Key("Description")]
        public string Description { get; set; }
    }
}
