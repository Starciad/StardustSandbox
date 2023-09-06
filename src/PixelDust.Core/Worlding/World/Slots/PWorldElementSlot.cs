using Microsoft.Xna.Framework;

using PixelDust.Core.Elements;
using PixelDust.Core.Utilities;

using System;

namespace PixelDust.Core.Worlding
{
    public struct PWorldElementSlot
    {
        public readonly PElement Instance => PElementsHandler.GetElementById<PElement>(id);
        public readonly bool IsEmpty => id == 0;
        public readonly short Temperature => temperature;

        private byte id;
        private short temperature;

        internal void Instantiate(uint id)
        {
            Instantiate(PElementsHandler.GetElementById(id));
        }
        internal void Instantiate(PElement value)
        {
            Reset();
            id = value.Id;
            temperature = value.DefaultTemperature;
        }
        internal void Destroy()
        {
            Reset();
            id = 0;
        }
        internal void Copy(PWorldElementSlot value)
        {
            this = value;
        }
        internal void SetTemperatureValue(int value)
        {
            temperature = PTemperature.Clamp(value);
        }

        private void Reset()
        {
            temperature = 0;
        }
    }
}