using StardustSandbox.Constants;
using StardustSandbox.Enums.Simulation;
using StardustSandbox.Interfaces;

namespace StardustSandbox.WorldSystem.Status
{
    internal sealed class Simulation : IResettable
    {
        internal SimulationSpeed CurrentSpeed => this.currentSpeed;

        private SimulationSpeed currentSpeed = SimulationSpeed.Normal;

        private byte frameDelayThreshold = SimulationConstants.NORMAL_SPEED_FRAME_DELAY;
        private byte remainingFrameDelay;

        public void Reset()
        {
            this.frameDelayThreshold = SimulationConstants.NORMAL_SPEED_FRAME_DELAY;
            this.remainingFrameDelay = 0;
        }

        internal void Update()
        {
            if (this.remainingFrameDelay > 0)
            {
                this.remainingFrameDelay--;
                return;
            }

            this.remainingFrameDelay = this.frameDelayThreshold;
        }

        internal void SetSpeed(SimulationSpeed speed)
        {
            this.currentSpeed = speed;

            this.frameDelayThreshold = speed switch
            {
                SimulationSpeed.Normal => SimulationConstants.NORMAL_SPEED_FRAME_DELAY,
                SimulationSpeed.Fast => SimulationConstants.FAST_SPEED_FRAME_DELAY,
                SimulationSpeed.VeryFast => SimulationConstants.VERY_FAST_SPEED_FRAME_DELAY,
                _ => SimulationConstants.NORMAL_SPEED_FRAME_DELAY,
            };
        }

        internal bool CanContinueExecution()
        {
            if (this.remainingFrameDelay == 0)
            {
                this.remainingFrameDelay = this.frameDelayThreshold;
                return true;
            }

            return false;
        }
    }
}
