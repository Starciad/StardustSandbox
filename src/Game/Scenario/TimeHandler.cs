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

using StardustSandbox.Constants;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.Scenario
{
    internal sealed class TimeHandler(Time time)
    {
        internal float IntervalProgress => this.intervalProgress;

        private float currentSeconds;
        private float intervalDuration;
        private float intervalProgress;
        private bool isDay;

        private readonly Time time = time;

        internal void Update()
        {
            this.currentSeconds = Convert.ToSingle(this.time.CurrentTime.TotalSeconds);

            UpdateDayState();
            UpdateIntervalDuration();
            UpdateIntervalProgres();
        }

        private void UpdateDayState()
        {
            // Determine if it's day or night
            this.isDay = this.currentSeconds >= TimeConstants.DAY_START_IN_SECONDS && this.currentSeconds < TimeConstants.NIGHT_START_IN_SECONDS;
        }

        private void UpdateIntervalDuration()
        {
            // Calculate normalized time for the active interval
            this.intervalDuration = this.isDay
                ? TimeConstants.NIGHT_START_IN_SECONDS - TimeConstants.DAY_START_IN_SECONDS // Day duration
                : TimeConstants.SECONDS_IN_A_DAY - (TimeConstants.NIGHT_START_IN_SECONDS - TimeConstants.DAY_START_IN_SECONDS); // Night duration
        }

        private void UpdateIntervalProgres()
        {
            this.intervalProgress = this.isDay
                ? (this.currentSeconds - TimeConstants.DAY_START_IN_SECONDS) / this.intervalDuration
                : this.currentSeconds >= TimeConstants.NIGHT_START_IN_SECONDS
                    ? (this.currentSeconds - TimeConstants.NIGHT_START_IN_SECONDS) / this.intervalDuration
                    : (this.currentSeconds + TimeConstants.SECONDS_IN_A_DAY - TimeConstants.NIGHT_START_IN_SECONDS) / this.intervalDuration;
        }
    }
}

