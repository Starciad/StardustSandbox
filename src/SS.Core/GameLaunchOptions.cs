namespace StardustSandbox.Core
{
    public sealed class GameLaunchOptions
    {
        public required bool CreateException { get; init; }
        public required bool NoMusicDelay { get; init; }
        public required bool ShowChunks { get; init; }
        public required bool SkipIntro { get; init; }
    }
}
