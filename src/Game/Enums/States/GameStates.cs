using System;

namespace StardustSandbox.Enums.States
{
    [Flags]
    internal enum GameStates : byte
    {
        None = 0,
        IsFocused = 1 << 0,
        IsPaused = 1 << 1,
        IsSimulationPaused = 1 << 2,
        IsCriticalMenuOpen = 1 << 3,
    }
}
