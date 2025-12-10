using StardustSandbox.Constants;
using StardustSandbox.WorldSystem.Status;

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
