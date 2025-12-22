using MessagePack;

using Microsoft.Xna.Framework;

using System;

namespace StardustSandbox.Serialization.Saving.Data
{
    [Serializable]
    [MessagePackObject]
    public sealed class PropertyData
    {
        [Key("Width")]
        public int Width { get; init; }

        [Key("Height")]
        public int Height { get; init; }

        [IgnoreMember]
        public Point Size => new(this.Width, this.Height);
    }
}
