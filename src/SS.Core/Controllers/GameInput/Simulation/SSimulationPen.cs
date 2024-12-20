﻿using StardustSandbox.Core.Constants;

namespace StardustSandbox.Core.Controllers.GameInput.Simulation
{
    public sealed class SSimulationPen
    {
        public byte Size => this.size;

        private byte size = 1;

        public void AddSize(byte value)
        {
            if (this.size + value > SInputConstants.PEN_MAX_SIZE)
            {
                this.size = SInputConstants.PEN_MAX_SIZE;
                return;
            }

            this.size += value;
        }

        public void RemoveSize(byte value)
        {
            if (this.size - value < SInputConstants.PEN_MIN_SIZE)
            {
                this.size = SInputConstants.PEN_MIN_SIZE;
                return;
            }

            this.size -= value;
        }

        public void SetSize(byte value)
        {
            this.size = byte.Clamp(value, SInputConstants.PEN_MIN_SIZE, SInputConstants.PEN_MAX_SIZE);
        }
    }
}
