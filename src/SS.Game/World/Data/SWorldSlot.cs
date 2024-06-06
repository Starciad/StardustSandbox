using StardustSandbox.Game.Elements;
using StardustSandbox.Game.Mathematics;

using System;

namespace StardustSandbox.Game.World.Data
{
    public struct SWorldSlot : ICloneable
    {
        public readonly uint Id => this.id;
        public readonly bool IsEmpty => this.isEmpty;
        public readonly short Temperature => this.temperature;

        private bool isEmpty;
        private uint id;
        private short temperature;

        public SWorldSlot()
        {
            Destroy();
        }
        public SWorldSlot(SWorldSlot value)
        {
            this = value;
        }

        public void Instantiate(SElement value)
        {
            this.isEmpty = false;
            this.id = value.Id;
            this.temperature = value.DefaultTemperature;
        }

        public void Destroy()
        {
            this.isEmpty = true;
            this.id = 0;
            this.temperature = 0;
        }

        public void SetTemperatureValue(int value)
        {
            this.temperature = STemperatureMath.Clamp(value);
        }

        public readonly object Clone()
        {
            return new SWorldSlot
            {
                isEmpty = this.isEmpty,
                id = this.id,
                temperature = this.temperature
            };
        }
    }
}