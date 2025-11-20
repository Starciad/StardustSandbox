using System;

namespace StardustSandbox.Enums.Elements
{
    [Flags]
    public enum ElementStates : byte
    {
        None = 0,
        IsEmpty = 1 << 0,
        FreeFalling = 1 << 1,
    }
}
