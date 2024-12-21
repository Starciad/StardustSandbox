using System;

namespace StardustSandbox.Core.Enums.World
{
    [Flags]
    public enum SWorldLayer : byte
    {
        None = 0,
        Foreground = 1,
        Background = 2,
    }
}
