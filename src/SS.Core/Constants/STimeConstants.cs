using System;

namespace StardustSandbox.Core.Constants
{
    public static class STimeConstants
    {
        public static TimeSpan DEFAULT_START_TIME_OF_DAY => new(06, 00, 00);

        public const int DEFAULT_SECONDS_PER_FRAMES = 10;

        public const int SECONDS_IN_A_DAY = 86400;
        public const int SECONDS_IN_AN_HOUR = 3600;
        public const int SECONDS_IN_A_MINUTE = 60;
    }
}
