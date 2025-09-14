using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.Simulation;
using StardustSandbox.Core.Interfaces.System;

namespace StardustSandbox.Core.World.General
{
    public sealed class SWorldSimulation : ISResettable
    {
        public SSimulationSpeed CurrentSpeed => this.currentSpeed;

        private SSimulationSpeed currentSpeed = SSimulationSpeed.Normal;

        private byte frameDelayThreshold = SSimulationConstants.NORMAL_SPEED_FRAME_DELAY;
        private byte remainingFrameDelay;

        public void Update()
        {
            if (this.remainingFrameDelay > 0)
            {
                this.remainingFrameDelay--;
                return;
            }

            this.remainingFrameDelay = this.frameDelayThreshold;
        }

        public void SetSpeed(SSimulationSpeed speed)
        {
            this.currentSpeed = speed;

            this.frameDelayThreshold = speed switch
            {
                SSimulationSpeed.Normal => SSimulationConstants.NORMAL_SPEED_FRAME_DELAY,
                SSimulationSpeed.Fast => SSimulationConstants.FAST_SPEED_FRAME_DELAY,
                SSimulationSpeed.VeryFast => SSimulationConstants.VERY_FAST_SPEED_FRAME_DELAY,
                _ => SSimulationConstants.NORMAL_SPEED_FRAME_DELAY,
            };
        }

        public bool CanContinueExecution()
        {
            if (this.remainingFrameDelay == 0)
            {
                this.remainingFrameDelay = this.frameDelayThreshold;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            this.frameDelayThreshold = SSimulationConstants.NORMAL_SPEED_FRAME_DELAY;
            this.remainingFrameDelay = 0;
        }
    }
}
