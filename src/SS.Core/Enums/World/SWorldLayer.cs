using System;

namespace StardustSandbox.Core.Enums.World
{
    [Flags]
    internal enum SWorldLayer
    {
        None = 0,
        Foreground = 1 << 0,
        Background = 1 << 1
    }
}
