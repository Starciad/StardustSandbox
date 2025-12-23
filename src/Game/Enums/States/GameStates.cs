using System;

namespace StardustSandbox.Enums.States
{
    [Flags]
    internal enum GameStates : byte
    {
        None = 0,
        IsLoading = 1 << 0,
        IsFocused = 1 << 1,
        IsPaused = 1 << 2,
        IsSimulationPaused = 1 << 3,
        IsCriticalMenuOpen = 1 << 4,
    }
}
