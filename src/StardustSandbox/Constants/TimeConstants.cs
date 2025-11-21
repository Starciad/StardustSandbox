using System;

namespace StardustSandbox.Constants
{
    internal static class TimeConstants
    {
        internal static TimeSpan DAY_START_TIMESPAN => new(06, 00, 00);

        internal const double DEFAULT_SECONDS_PER_FRAMES = 128.0;
        internal const double DEFAULT_FAST_SECONDS_PER_FRAMES = 512.0;
        internal const double DEFAULT_VERY_FAST_SECONDS_PER_FRAMES = 2048.0;

        internal const double DAY_START_IN_SECONDS = SECONDS_IN_A_DAY * 0.25; // 6:00 AM
        internal const double NIGHT_START_IN_SECONDS = SECONDS_IN_A_DAY * 0.75; // 6:00 PM

        internal const double SECONDS_IN_A_DAY = 86400.0;
        internal const double SECONDS_IN_AN_HOUR = 3600.0;
        internal const double SECONDS_IN_A_MINUTE = 60.0;
    }
}
