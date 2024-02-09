using PixelDust.Core.Elements;
using PixelDust.Core.Utilities;

namespace PixelDust.Core.Worlding.World.Slots
{
    public struct PWorldElementSlot
    {
        public readonly PElement Instance => PElementsHandler.GetElementById<PElement>(this.id);
        public readonly bool IsEmpty => this.id == 0;
        public readonly short Temperature => this.temperature;

        private byte id;
        private short temperature;

        internal void Instantiate(uint id)
        {
            Instantiate(PElementsHandler.GetElementById(id));
        }
        internal void Instantiate(PElement value)
        {
            Reset();
            this.id = value.Id;
            this.temperature = value.DefaultTemperature;
        }
        internal void Destroy()
        {
            Reset();
            this.id = 0;
        }
        internal void Copy(PWorldElementSlot value)
        {
            this = value;
        }
        internal void SetTemperatureValue(int value)
        {
            this.temperature = PTemperature.Clamp(value);
        }

        private void Reset()
        {
            SetTemperatureValue(0);
        }
    }
}