using Microsoft.Xna.Framework;

using StardustSandbox.Game.Elements;
using StardustSandbox.Game.Interfaces.General;
using StardustSandbox.Game.Mathematics;

namespace StardustSandbox.Game.World.Data
{
    public sealed class SWorldSlot : ISPoolableObject
    {
        public SElement Element => this.element;
        public bool IsEmpty => this.isEmpty;
        public short Temperature => this.temperature;
        public bool FreeFalling => this.freeFalling;
        public Color Color => this.color;

        private bool isEmpty;
        private short temperature;
        private bool freeFalling;
        private Color color;

        private SElement element;

        public SWorldSlot()
        {
            Reset();
        }

        public void Instantiate(SElement value)
        {
            this.isEmpty = false;
            this.temperature = value.DefaultTemperature;
            this.freeFalling = false;
            this.color = Color.White;

            this.element = value;
            this.element?.AwakeStep(this);
        }

        public void Destroy()
        {
            this.isEmpty = true;
            this.temperature = 0;
            this.freeFalling = false;
            this.element = null;
            this.color = Color.Transparent;
        }

        public void Copy(SWorldSlot value)
        {
            this.isEmpty = value.isEmpty;
            this.temperature = value.temperature;
            this.freeFalling = value.freeFalling;
            this.color = value.color;

            this.element = value.element;
        }

        public void SetTemperatureValue(int value)
        {
            this.temperature = STemperatureMath.Clamp(value);
        }

        public void SetFreeFalling(bool value)
        {
            this.freeFalling = value;
        }

        public void SetColor(Color color)
        {
            this.color = color;
        }

        public void Reset()
        {
            Destroy();
        }
    }
}