using PixelDust.Game.Elements;
using PixelDust.Game.Utilities;

namespace PixelDust.Game.Worlding.World.Slots
{
    public struct PWorldElementSlot
    {
        public readonly PElement Instance => PElementDatabase.GetElementById<PElement>(this.id);
        public readonly byte Depth => this.depth;
        public readonly bool IsEmpty => this.id == 0;
        public readonly short Temperature => this.temperature;

        private byte id;
        private byte depth;
        private short temperature;

        public void Instantiate(uint id)
        {
            Instantiate(PElementDatabase.GetElementById(id));
        }
        public void Instantiate(PElement value)
        {
            Reset();
            this.id = value.Id;
            this.temperature = value.DefaultTemperature;
        }
        public void Destroy()
        {
            Reset();
            this.id = 0;
        }
        public void Copy(PWorldElementSlot value)
        {
            this = value;
        }
        public void SetTemperatureValue(int value)
        {
            this.temperature = PTemperature.Clamp(value);
        }

        private void Reset()
        {
            SetTemperatureValue(0);
        }
    }
}