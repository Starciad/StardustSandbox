using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Ambient.Handlers;
using StardustSandbox.Core.Objects;
using StardustSandbox.Core.World.General;

namespace StardustSandbox.Core.Ambient.Handlers
{
    internal sealed class STimeHandler(ISGame gameInstance) : SGameObject(gameInstance), ISTimeHandler
    {
        public float GlobalIllumination => this.globalIllumination;
        public double CurrentSeconds => this.currentSeconds;
        public double IntervalDuration => this.intervalDuration;
        public double IntervalProgress => this.intervalProgress;

        private float globalIllumination = 1.0f;
        private double currentSeconds;
        private double intervalDuration;
        private double intervalProgress;
        private bool isDay;

        private readonly SWorldTime worldTime = gameInstance.World.Time;

        public override void Update(GameTime gameTime)
        {
            this.currentSeconds = this.worldTime.CurrentTime.TotalSeconds;

            UpdateDayState();
            UpdateIntervalDuration();
            UpdateGlobalIllumination();
            UpdateIntervalProgres();
        }

        private void UpdateDayState()
        {
            // Determine if it's day or night
            this.isDay = this.currentSeconds >= STimeConstants.DAY_START_IN_SECONDS && this.currentSeconds < STimeConstants.NIGHT_START_IN_SECONDS;
        }

        private void UpdateIntervalDuration()
        {
            // Calculate normalized time for the active interval
            this.intervalDuration = this.isDay
                ? STimeConstants.NIGHT_START_IN_SECONDS - STimeConstants.DAY_START_IN_SECONDS // Day duration
                : STimeConstants.SECONDS_IN_A_DAY - (STimeConstants.NIGHT_START_IN_SECONDS - STimeConstants.DAY_START_IN_SECONDS); // Night duration
        }

        private void UpdateGlobalIllumination()
        {
            // Calculate global illumination
            if (this.isDay)
            {
                // Illumination increases in the morning and decreases in the afternoon
                this.globalIllumination = MathHelper.Lerp(0.5f, 1.0f, (float)(this.intervalProgress <= 0.5 ? this.intervalProgress * 2 : (1 - this.intervalProgress) * 2));
            }
            else
            {
                // Illumination decreases during the night
                this.globalIllumination = MathHelper.Lerp(0.1f, 0.5f, (float)(1 - this.intervalProgress));
            }
        }

        private void UpdateIntervalProgres()
        {
            this.intervalProgress = this.isDay
                ? (this.currentSeconds - STimeConstants.DAY_START_IN_SECONDS) / this.intervalDuration
                : this.currentSeconds >= STimeConstants.NIGHT_START_IN_SECONDS
                    ? (this.currentSeconds - STimeConstants.NIGHT_START_IN_SECONDS) / this.intervalDuration
                    : (this.currentSeconds + STimeConstants.SECONDS_IN_A_DAY - STimeConstants.NIGHT_START_IN_SECONDS) / this.intervalDuration;
        }
    }
}
