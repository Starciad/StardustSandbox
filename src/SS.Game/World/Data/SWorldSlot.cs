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
        public readonly bool FreeFalling => this.freeFalling;

        private bool isEmpty;
        private uint id;
        private short temperature;
        private bool freeFalling;

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
            this.freeFalling = false;
        }

        public void Destroy()
        {
            this.isEmpty = true;
            this.id = 0;
            this.temperature = 0;
            this.freeFalling = false;
        }

        public void SetTemperatureValue(int value)
        {
            this.temperature = STemperatureMath.Clamp(value);
        }

        public void SetFreeFalling(bool value)
        {
            this.freeFalling = value;
        }

        public readonly object Clone()
        {
            return new SWorldSlot
            {
                isEmpty = this.isEmpty,
                id = this.id,
                temperature = this.temperature,
                freeFalling = this.freeFalling,
            };
        }
    }
}