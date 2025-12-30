using System;

namespace StardustSandbox.Enums.Serialization
{
    [Flags]
    internal enum LoadFlags : byte
    {
        None = 0,
        Thumbnail = 1 << 0,
        Metadata = 1 << 1,
        Manifest = 1 << 2,
        Properties = 1 << 3,
        Environment = 1 << 4,
        Content = 1 << 5,

        All = Thumbnail | Metadata | Manifest | Properties | Environment | Content
    }
}
