using System;

namespace StardustSandbox.Enums.Generators
{
    [Flags]
    internal enum WorldGenerationFlags : byte
    {
        None = 0,
        HasTrees = 1 << 0,
        HasCaves = 1 << 1,
    }
}
