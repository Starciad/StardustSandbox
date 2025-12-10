using Microsoft.Xna.Framework;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Simulation;
using StardustSandbox.Interfaces;

using System;

namespace StardustSandbox.WorldSystem.Status
{
    internal sealed class Simulation : IResettable
    {
        internal SimulationSpeed CurrentSpeed => this.currentSpeed;

        private SimulationSpeed currentSpeed = SimulationSpeed.Normal;

        private float delayThresholdSeconds = SimulationConstants.NORMAL_SPEED_DELAY_SECONDS;
        private float accumulatedTimeSeconds;

        public void Reset()
        {
            this.delayThresholdSeconds = SimulationConstants.NORMAL_SPEED_DELAY_SECONDS;
            this.accumulatedTimeSeconds = 0.0f;
        }

        internal void Update(in GameTime gameTime)
        {
            this.accumulatedTimeSeconds += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
        }

        internal void SetSpeed(SimulationSpeed speed)
        {
            this.currentSpeed = speed;
            this.delayThresholdSeconds = speed switch
            {
                SimulationSpeed.Normal => SimulationConstants.NORMAL_SPEED_DELAY_SECONDS,
                SimulationSpeed.Fast => SimulationConstants.FAST_SPEED_DELAY_SECONDS,
                SimulationSpeed.VeryFast => SimulationConstants.VERY_FAST_SPEED_DELAY_SECONDS,
                _ => SimulationConstants.NORMAL_SPEED_DELAY_SECONDS,
            };
        }

        internal bool CanContinueExecution()
        {
            if (this.accumulatedTimeSeconds >= this.delayThresholdSeconds)
            {
                this.accumulatedTimeSeconds = 0.0f;
                return true;
            }

            return false;
        }
    }
}
