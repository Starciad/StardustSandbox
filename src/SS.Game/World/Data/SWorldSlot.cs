using StardustSandbox.Game.Elements;
using StardustSandbox.Game.Interfaces.General;
using StardustSandbox.Game.Mathematics;

namespace StardustSandbox.Game.World.Data
{
    public sealed class SWorldSlot : ISPoolableObject
    {
        public SElement Element => this.element;
        public uint Id => this.id;
        public bool IsEmpty => this.isEmpty;
        public short Temperature => this.temperature;
        public bool FreeFalling => this.freeFalling;

        private bool isEmpty;
        private uint id;
        private short temperature;
        private bool freeFalling;
        private SElement element;

        public SWorldSlot()
        {
            Reset();
        }

        public void Copy(SWorldSlot value)
        {
            this.isEmpty = value.isEmpty;
            this.id = value.id;
            this.temperature = value.temperature;
            this.freeFalling = value.freeFalling;
            this.element = value.element;
        }

        public void Instantiate(SElement value)
        {
            this.isEmpty = false;
            this.id = value.Id;
            this.temperature = value.DefaultTemperature;
            this.freeFalling = false;
            this.element = value;
        }

        public void Destroy()
        {
            this.isEmpty = true;
            this.id = 0;
            this.temperature = 0;
            this.freeFalling = false;
            this.element = null;
        }

        public void SetTemperatureValue(int value)
        {
            this.temperature = STemperatureMath.Clamp(value);
        }

        public void SetFreeFalling(bool value)
        {
            this.freeFalling = value;
        }

        public void Reset()
        {
            Destroy();
        }
    }
}