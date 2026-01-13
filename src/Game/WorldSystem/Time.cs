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

using Microsoft.Xna.Framework;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Simulation;
using StardustSandbox.Interfaces;

using System;

namespace StardustSandbox.WorldSystem
{
    internal sealed class Time : IResettable
    {
        internal TimeSpan CurrentTime => this.currentTime;
        internal float InGameSecondsPerRealSecond { get; set; } = TimeConstants.DEFAULT_SECONDS_PER_FRAMES;
        internal bool IsFrozen { get; set; } = false;

        private TimeSpan currentTime = TimeConstants.DAY_START_TIMESPAN;

        public void Reset()
        {
            this.IsFrozen = false;
            this.InGameSecondsPerRealSecond = TimeConstants.DEFAULT_SECONDS_PER_FRAMES;
            this.currentTime = TimeConstants.DAY_START_TIMESPAN;
        }

        internal void Update(GameTime gameTime)
        {
            if (this.IsFrozen)
            {
                return;
            }

            // Uses deltaTime to update the time independently of the FPS.
            this.currentTime = this.currentTime.Add(TimeSpan.FromSeconds(this.InGameSecondsPerRealSecond * gameTime.ElapsedGameTime.TotalSeconds));

            // Normalizes the time to 24 hours.
            this.currentTime = TimeSpan.FromSeconds(this.currentTime.TotalSeconds % TimeConstants.SECONDS_IN_A_DAY);
        }

        internal DayPeriod GetCurrentDayPeriod()
        {
            return this.currentTime >= TimeSpan.FromHours(0) && this.currentTime < TimeSpan.FromHours(6)
                ? DayPeriod.AnteLucan
                : this.currentTime >= TimeSpan.FromHours(6) && this.currentTime < TimeSpan.FromHours(12)
                ? DayPeriod.Morning
                : this.currentTime >= TimeSpan.FromHours(12) && this.currentTime < TimeSpan.FromHours(18)
                ? DayPeriod.Afternoon
                : this.currentTime >= TimeSpan.FromHours(18) && this.currentTime < TimeSpan.FromHours(24)
                ? DayPeriod.Night
                : throw new InvalidOperationException($"Invalid time state in {nameof(GetCurrentDayPeriod)}.");
        }

        internal void SetTime(TimeSpan value)
        {
            this.currentTime = value;
        }

        internal void SetSpeed(SimulationSpeed speed)
        {
            this.InGameSecondsPerRealSecond = speed switch
            {
                SimulationSpeed.Normal => TimeConstants.DEFAULT_SECONDS_PER_FRAMES,
                SimulationSpeed.Fast => TimeConstants.DEFAULT_FAST_SECONDS_PER_FRAMES,
                SimulationSpeed.VeryFast => TimeConstants.DEFAULT_VERY_FAST_SECONDS_PER_FRAMES,
                _ => TimeConstants.DEFAULT_SECONDS_PER_FRAMES,
            };
        }
    }
}

