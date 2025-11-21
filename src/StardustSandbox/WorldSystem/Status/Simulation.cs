using Microsoft.Xna.Framework;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Simulation;
using StardustSandbox.Interfaces;

namespace StardustSandbox.WorldSystem.Status
{
    internal sealed class Simulation : IResettable
    {
        internal SimulationSpeed CurrentSpeed => this.currentSpeed;

        private SimulationSpeed currentSpeed = SimulationSpeed.Normal;

        private double delayThresholdSeconds = SimulationConstants.NORMAL_SPEED_DELAY_SECONDS;
        private double accumulatedTimeSeconds;

        public void Reset()
        {
            this.delayThresholdSeconds = SimulationConstants.NORMAL_SPEED_DELAY_SECONDS;
            this.accumulatedTimeSeconds = 0.0;
        }

        internal void Update(GameTime gameTime)
        {
            this.accumulatedTimeSeconds += gameTime.ElapsedGameTime.TotalSeconds;
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
                this.accumulatedTimeSeconds -= this.delayThresholdSeconds;
                return true;
            }

            return false;
        }
    }
}
