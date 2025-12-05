using System;

namespace StardustSandbox.Constants
{
    internal static class TimeConstants
    {
        internal static TimeSpan DAY_START_TIMESPAN => new(06, 00, 00);

        internal const float DEFAULT_SECONDS_PER_FRAMES = 128.0f;
        internal const float DEFAULT_FAST_SECONDS_PER_FRAMES = 512.0f;
        internal const float DEFAULT_VERY_FAST_SECONDS_PER_FRAMES = 2048.0f;

        internal const float DAY_START_IN_SECONDS = SECONDS_IN_A_DAY * 0.25f; // 6:00 AM
        internal const float NIGHT_START_IN_SECONDS = SECONDS_IN_A_DAY * 0.75f; // 6:00 PM

        internal const float SECONDS_IN_A_DAY = 86400.0f;
        internal const float SECONDS_IN_AN_HOUR = 3600.0f;
        internal const float SECONDS_IN_A_MINUTE = 60.0f;
    }
}
