using System;

namespace StardustSandbox.Constants
{
    internal static class TimeConstants
    {
        internal static TimeSpan DEFAULT_START_TIME_OF_DAY => new(06, 00, 00);

        internal const float DEFAULT_NORMAL_SECONDS_PER_FRAMES = 2.0f;
        internal const float DEFAULT_FAST_SECONDS_PER_FRAMES = 8.0f;
        internal const float DEFAULT_VERY_FAST_SECONDS_PER_FRAMES = 16.0f;

        internal const float DAY_START_IN_SECONDS = SECONDS_IN_A_DAY * 0.25f; // 6:00 AM
        internal const float NIGHT_START_IN_SECONDS = SECONDS_IN_A_DAY * 0.75f; // 6:00 PM

        internal const int SECONDS_IN_A_DAY = 86400;
        internal const int SECONDS_IN_AN_HOUR = 3600;
        internal const int SECONDS_IN_A_MINUTE = 60;
    }
}
