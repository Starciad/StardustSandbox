namespace StardustSandbox.Core
{
    public sealed class SGameState
    {
        public bool IsFocused { get; set; }
        public bool IsPaused { get; set; }
        public bool IsSimulationPaused { get; set; }
        public bool IsCriticalMenuOpen { get; set; }
    }
}
