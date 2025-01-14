using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.System;
using StardustSandbox.Core.Objects;

using System;

namespace StardustSandbox.Core.World.General
{
    public sealed class SWorldTime(ISGame gameInstance) : SGameObject(gameInstance), ISResettable
    {
        public TimeSpan CurrentTime => this.currentTime;
        public float SecondsPerFrames { get; set; } = STimeConstants.DEFAULT_NORMAL_SECONDS_PER_FRAMES;
        public bool IsFrozen { get; set; } = false;

        private TimeSpan currentTime = STimeConstants.DEFAULT_START_TIME_OF_DAY;

        public override void Update(GameTime gameTime)
        {
            if (this.IsFrozen)
            {
                return;
            }

            this.currentTime = this.currentTime.Add(TimeSpan.FromSeconds(this.SecondsPerFrames));

            // Normalizes time to 24 hours.
            this.currentTime = TimeSpan.FromSeconds(this.currentTime.TotalSeconds % STimeConstants.SECONDS_IN_A_DAY);
        }

        public SDayPeriod GetCurrentDayPeriod()
        {
            return this.currentTime >= TimeSpan.FromHours(0) && this.currentTime < TimeSpan.FromHours(6)
                ? SDayPeriod.AnteLucan
                : this.currentTime >= TimeSpan.FromHours(6) && this.currentTime < TimeSpan.FromHours(12)
                ? SDayPeriod.Morning
                : this.currentTime >= TimeSpan.FromHours(12) && this.currentTime < TimeSpan.FromHours(18)
                ? SDayPeriod.Afternoon
                : this.currentTime >= TimeSpan.FromHours(18) && this.currentTime < TimeSpan.FromHours(24)
                ? SDayPeriod.Night
                : throw new InvalidOperationException($"Invalid time state in {nameof(GetCurrentDayPeriod)}.");
        }

        public void SetTime(TimeSpan value)
        {
            this.currentTime = value;
        }

        public void Reset()
        {
            this.IsFrozen = false;
            this.SecondsPerFrames = STimeConstants.DEFAULT_NORMAL_SECONDS_PER_FRAMES;
            this.currentTime = STimeConstants.DEFAULT_START_TIME_OF_DAY;
        }
    }
}
