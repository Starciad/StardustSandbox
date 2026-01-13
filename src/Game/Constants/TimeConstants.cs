/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

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

