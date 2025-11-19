using Microsoft.Xna.Framework;

using StardustSandbox.Elements;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Extensions;
using StardustSandbox.Mathematics;

namespace StardustSandbox.WorldSystem
{
    public sealed class SlotLayer
    {
        internal Element Element => this.element;
        internal Element StoredElement => this.storedElement;
        internal bool IsEmpty => this.isEmpty;
        internal short Temperature => this.temperature;
        internal bool FreeFalling => this.freeFalling;
        internal Color ColorModifier => this.colorModifier;
        internal UpdateCycleFlag UpdateCycleFlag => this.updateCycleFlag;
        internal UpdateCycleFlag StepCycleFlag => this.stepCycleFlag;

        private Element element;
        private Element storedElement;
        private bool isEmpty;
        private short temperature;
        private bool freeFalling;
        private Color colorModifier;
        private UpdateCycleFlag updateCycleFlag;
        private UpdateCycleFlag stepCycleFlag;

        internal SlotLayer()
        {
            Reset();
        }

        internal void Instantiate(Element value)
        {
            this.isEmpty = false;
            this.element = value;
            this.storedElement = null;
            this.temperature = value.DefaultTemperature;
            this.freeFalling = false;
            this.colorModifier = Color.White;
            this.updateCycleFlag = UpdateCycleFlag.None;
            this.stepCycleFlag = UpdateCycleFlag.None;
        }

        internal void Destroy()
        {
            this.isEmpty = true;
            this.element = null;
            this.storedElement = null;
            this.temperature = 0;
            this.freeFalling = false;
            this.colorModifier = Color.White;
            this.updateCycleFlag = UpdateCycleFlag.None;
            this.stepCycleFlag = UpdateCycleFlag.None;
        }

        internal void Copy(SlotLayer valueToCopy)
        {
            this.element = valueToCopy.element;
            this.storedElement = valueToCopy.storedElement;
            this.isEmpty = valueToCopy.isEmpty;
            this.temperature = valueToCopy.temperature;
            this.freeFalling = valueToCopy.freeFalling;
            this.colorModifier = valueToCopy.colorModifier;
            this.updateCycleFlag = valueToCopy.updateCycleFlag;
            this.stepCycleFlag = valueToCopy.stepCycleFlag;
        }

        internal void SetTemperatureValue(short value)
        {
            this.temperature = TemperatureMath.Clamp(value);
        }

        internal void SetFreeFalling(bool value)
        {
            this.freeFalling = value;
        }

        internal void SetColorModifier(Color value)
        {
            this.colorModifier = value;
        }

        internal void NextUpdateCycle()
        {
            this.updateCycleFlag = this.updateCycleFlag.GetNextCycle();
        }

        internal void NextStepCycle()
        {
            this.stepCycleFlag = this.stepCycleFlag.GetNextCycle();
        }

        internal void SetStoredElement(Element value)
        {
            this.storedElement = value;
        }

        public void Reset()
        {
            Destroy();
        }
    }
}
