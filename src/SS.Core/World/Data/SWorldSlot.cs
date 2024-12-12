using Microsoft.Xna.Framework;

using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Mathematics;

namespace StardustSandbox.Core.World.Data
{
    internal sealed class SWorldSlot : ISWorldSlot
    {
        public ISElement Element => this.element;
        public Point Position => this.position;
        public bool IsEmpty => this.isEmpty;
        public short Temperature => this.temperature;
        public bool FreeFalling => this.freeFalling;
        public Color Color => this.color;

        private bool isEmpty;
        private Point position;
        private short temperature;
        private bool freeFalling;
        private Color color;

        private ISElement element;

        public SWorldSlot()
        {
            Reset();
        }

        public void Instantiate(Point position, ISElement value)
        {
            this.isEmpty = false;
            this.position = position;
            this.element = value;
            this.temperature = value.DefaultTemperature;
            this.freeFalling = false;
            this.color = Color.White;
        }

        public void Destroy()
        {
            this.isEmpty = true;
            this.position = Point.Zero;
            this.element = null;
            this.temperature = 0;
            this.freeFalling = false;
            this.color = Color.Transparent;
        }

        public void Copy(SWorldSlot value)
        {
            this.element = value.element;
            this.isEmpty = value.isEmpty;
            this.position = value.position;
            this.temperature = value.temperature;
            this.freeFalling = value.freeFalling;
            this.color = value.color;
        }

        public void SetPosition(Point value)
        {
            this.position = value;
        }

        public void SetTemperatureValue(int value)
        {
            this.temperature = STemperatureMath.Clamp(value);
        }

        public void SetFreeFalling(bool value)
        {
            this.freeFalling = value;
        }

        public void SetColor(Color value)
        {
            this.color = value;
        }

        public void Reset()
        {
            Destroy();
        }
    }
}