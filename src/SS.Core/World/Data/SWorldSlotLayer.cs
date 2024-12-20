using Microsoft.Xna.Framework;

using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Mathematics;

namespace StardustSandbox.Core.World.Data
{
    internal sealed class SWorldSlotLayer : ISWorldSlotLayer
    {
        public ISElement Element => this.element;
        public bool IsEmpty => this.isEmpty;
        public short Temperature => this.temperature;
        public bool FreeFalling => this.freeFalling;
        public Color ColorModifier => this.colorModifier;

        private ISElement element;
        private bool isEmpty;
        private short temperature;
        private bool freeFalling;
        private Color colorModifier;

        internal SWorldSlotLayer()
        {
            Reset();
        }

        internal void Instantiate(ISElement value)
        {
            this.isEmpty = false;
            this.element = value;
            this.temperature = value.DefaultTemperature;
            this.freeFalling = false;
            this.colorModifier = Color.White;
        }

        internal void Destroy()
        {
            this.isEmpty = true;
            this.element = null;
            this.temperature = 0;
            this.freeFalling = false;
            this.colorModifier = Color.White;
        }

        internal void Copy(ISWorldSlotLayer valueToCopy)
        {
            this.element = valueToCopy.Element;
            this.isEmpty = valueToCopy.IsEmpty;
            this.temperature = valueToCopy.Temperature;
            this.freeFalling = valueToCopy.FreeFalling;
            this.colorModifier = valueToCopy.ColorModifier;
        }

        internal void SetTemperatureValue(short value)
        {
            this.temperature = STemperatureMath.Clamp(value);
        }

        internal void SetFreeFalling(bool value)
        {
            this.freeFalling = value;
        }

        internal void SetColorModifier(Color value)
        {
            this.colorModifier = value;
        }

        public void Reset()
        {
            Destroy();
        }
    }
}
