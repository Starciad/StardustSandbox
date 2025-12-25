using System;

namespace StardustSandbox.Enums.Elements
{
    [Flags]
    public enum ElementStates : byte
    {
        None = 0,
        IsFalling = 1 << 0,
    }
}
