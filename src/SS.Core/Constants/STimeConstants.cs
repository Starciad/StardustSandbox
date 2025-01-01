using System;

namespace StardustSandbox.Core.Constants
{
    public static class STimeConstants
    {
        public static TimeSpan DEFAULT_START_TIME_OF_DAY => new(06, 00, 00);

        public const float DEFAULT_SECONDS_PER_FRAMES = 10f;

        public const float DAY_START_IN_SECONDS = SECONDS_IN_A_DAY * 0.25f; // 6:00 AM
        public const float NIGHT_START_IN_SECONDS = SECONDS_IN_A_DAY * 0.75f; // 6:00 PM

        public const int SECONDS_IN_A_DAY = 86400;
        public const int SECONDS_IN_AN_HOUR = 3600;
        public const int SECONDS_IN_A_MINUTE = 60;
    }
}
