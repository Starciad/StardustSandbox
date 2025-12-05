using Microsoft.Xna.Framework;

using StardustSandbox.Constants;
using StardustSandbox.World.Status;

using System;

namespace StardustSandbox.Scenario
{
    internal sealed class TimeHandler(Time time)
    {
        internal float GlobalIllumination => this.globalIllumination;
        internal float CurrentSeconds => this.currentSeconds;
        internal float IntervalDuration => this.intervalDuration;
        internal float IntervalProgress => this.intervalProgress;

        private float globalIllumination = 1.0f;
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
            UpdateGlobalIllumination();
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

        private void UpdateGlobalIllumination()
        {
            // Calculate global illumination
            if (this.isDay)
            {
                // Illumination increases in the morning and decreases in the afternoon
                this.globalIllumination = MathHelper.Lerp(0.5f, 1.0f, this.intervalProgress <= 0.5 ? this.intervalProgress * 2 : (1 - this.intervalProgress) * 2);
            }
            else
            {
                // Illumination decreases during the night
                this.globalIllumination = MathHelper.Lerp(0.1f, 0.5f, 1.0f - this.intervalProgress);
            }
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
