using MessagePack;

using Microsoft.Xna.Framework;

namespace StardustSandbox.Serialization.Saving.Data
{
    [MessagePackObject]
    public sealed class PropertiesData
    {
        [IgnoreMember]
        public Point Size => new(this.Width, this.Height);

        [Key("Width")]
        public int Width { get; set; }

        [Key("Height")]
        public int Height { get; set; }
    }
}
