using StardustSandbox.Enums.Assets;

namespace StardustSandbox.Constants
{
    internal static class SongConstants
    {
        internal const float FADE_STEP_INTERVAL_MS = 50.0f;
        internal const float FADE_DURATION_MS = 1500.0f;

        internal static readonly SongIndex[] GAMEPLAY_SONGS =
        [
            SongIndex.Volume_01_Track_03,
            SongIndex.Volume_01_Track_04,
            SongIndex.Volume_01_Track_05,
            SongIndex.Volume_01_Track_06,
        ];
    }
}
