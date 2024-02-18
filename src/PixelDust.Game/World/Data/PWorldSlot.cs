using PixelDust.Game.Elements;
using PixelDust.Game.Utilities;

using System;

namespace PixelDust.Game.World.Data
{
    public struct PWorldSlot : ICloneable
    {
        public readonly uint Id => this.id;
        public readonly bool IsEmpty => this.isEmpty;
        public readonly short Temperature => this.temperature;

        private bool isEmpty;
        private uint id;
        private short temperature;

        public PWorldSlot()
        {
            Destroy();
        }
        public PWorldSlot(PWorldSlot value)
        {
            this = value;
        }

        public void Instantiate(PElement value)
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
            this.temperature = PTemperature.Clamp(value);
        }

        public readonly object Clone()
        {
            return new PWorldSlot
            {
                isEmpty = this.isEmpty,
                id = this.id,
                temperature = this.temperature
            };
        }
    }
}