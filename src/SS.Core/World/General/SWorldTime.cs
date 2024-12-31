using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Objects;

using System;

namespace StardustSandbox.Core.World.General
{
    public sealed class SWorldTime(ISGame gameInstance) : SGameObject(gameInstance)
    {
        public TimeSpan CurrentTime => this.currentTime;
        public int TicksPerSecond { get; } = 60;

        private TimeSpan currentTime = TimeSpan.Zero;

        public override void Update(GameTime gameTime)
        {
            double elapsedTicks = gameTime.ElapsedGameTime.TotalSeconds * this.TicksPerSecond;
            this.currentTime = this.currentTime.Add(TimeSpan.FromTicks((long)elapsedTicks));

            // Normalizes time to 24 hours.
            this.currentTime = TimeSpan.FromSeconds(this.currentTime.TotalSeconds % STimeConstants.SECONDS_IN_A_DAY);
        }

        public void Define(int hours, int minutes, int seconds)
        {
            this.currentTime = new(hours, minutes, seconds);
        }

        public float GetNormalizedTime()
        {
            return (float)(this.currentTime.TotalSeconds % STimeConstants.SECONDS_IN_A_DAY) / STimeConstants.SECONDS_IN_A_DAY;
        }
    }
}
