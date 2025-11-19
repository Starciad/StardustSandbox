using StardustSandbox.Constants;
using StardustSandbox.Enums.Simulation;
using StardustSandbox.Interfaces;

using System;

namespace StardustSandbox.WorldSystem.Status
{
    internal sealed class Time : IResettable
    {
        internal TimeSpan CurrentTime => this.currentTime;
        internal float SecondsPerFrames { get; set; } = TimeConstants.DEFAULT_NORMAL_SECONDS_PER_FRAMES;
        internal bool IsFrozen { get; set; } = false;

        private TimeSpan currentTime = TimeConstants.DEFAULT_START_TIME_OF_DAY;

        public void Reset()
        {
            this.IsFrozen = false;
            this.SecondsPerFrames = TimeConstants.DEFAULT_NORMAL_SECONDS_PER_FRAMES;
            this.currentTime = TimeConstants.DEFAULT_START_TIME_OF_DAY;
        }

        internal void Update()
        {
            if (this.IsFrozen)
            {
                return;
            }

            this.currentTime = this.currentTime.Add(TimeSpan.FromSeconds(this.SecondsPerFrames));

            // Normalizes time to 24 hours.
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
    }
}
