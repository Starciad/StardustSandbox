using Microsoft.Xna.Framework;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Simulation;
using StardustSandbox.Interfaces;

using System;

namespace StardustSandbox.World.Status
{
    internal sealed class Time : IResettable
    {
        internal TimeSpan CurrentTime => this.currentTime;
        internal double InGameSecondsPerRealSecond { get; set; } = TimeConstants.DEFAULT_SECONDS_PER_FRAMES;
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
    }
}
