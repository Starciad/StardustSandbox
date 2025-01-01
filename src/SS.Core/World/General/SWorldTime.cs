using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Objects;

using System;

namespace StardustSandbox.Core.World.General
{
    public sealed class SWorldTime(ISGame gameInstance) : SGameObject(gameInstance)
    {
        public TimeSpan CurrentTime => this.currentTime;

        private TimeSpan currentTime = TimeSpan.Zero;

        public override void Update(GameTime gameTime)
        {
            this.currentTime = this.currentTime.Add(TimeSpan.FromSeconds(15));

            // Normalizes time to 24 hours.
            this.currentTime = TimeSpan.FromSeconds(this.currentTime.TotalSeconds % STimeConstants.SECONDS_IN_A_DAY);
            Console.WriteLine(this.currentTime);
        }

        public void Define(int hours, int minutes, int seconds)
        {
            this.currentTime = new(hours, minutes, seconds);
        }

        public float GetNormalizedTime()
        {
            return (float)(this.currentTime.TotalSeconds % STimeConstants.SECONDS_IN_A_DAY) / STimeConstants.SECONDS_IN_A_DAY;
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
    }
}
