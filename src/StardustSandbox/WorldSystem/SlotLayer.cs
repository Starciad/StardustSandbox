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
        internal ElementStates States => this.states;
        internal short Temperature => this.temperature;
        internal Color ColorModifier => this.colorModifier;
        internal UpdateCycleFlag UpdateCycleFlag => this.updateCycleFlag;
        internal UpdateCycleFlag StepCycleFlag => this.stepCycleFlag;

        private Element element;
        private Element storedElement;
        private ElementStates states;
        private short temperature;
        private Color colorModifier;
        private UpdateCycleFlag updateCycleFlag;
        private UpdateCycleFlag stepCycleFlag;

        internal SlotLayer()
        {
            Reset();
        }

        internal void Instantiate(Element value)
        {
            ClearStates();

            this.element = value;
            this.storedElement = null;
            this.temperature = value.DefaultTemperature;
            this.colorModifier = Color.White;
            this.updateCycleFlag = UpdateCycleFlag.None;
            this.stepCycleFlag = UpdateCycleFlag.None;
        }

        internal void Destroy()
        {
            ClearStates();
            SetState(ElementStates.IsEmpty);

            this.element = null;
            this.storedElement = null;
            this.temperature = 0;
            this.colorModifier = Color.White;
            this.updateCycleFlag = UpdateCycleFlag.None;
            this.stepCycleFlag = UpdateCycleFlag.None;
        }

        internal void Copy(SlotLayer valueToCopy)
        {
            this.element = valueToCopy.element;
            this.storedElement = valueToCopy.storedElement;
            this.states = valueToCopy.states;
            this.temperature = valueToCopy.temperature;
            this.colorModifier = valueToCopy.colorModifier;
            this.updateCycleFlag = valueToCopy.updateCycleFlag;
            this.stepCycleFlag = valueToCopy.stepCycleFlag;
        }

        internal void SetTemperatureValue(short value)
        {
            this.temperature = TemperatureMath.Clamp(value);
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

        internal bool HasState(ElementStates value)
        {
            return this.states.HasFlag(value);
        }

        internal void SetState(ElementStates value)
        {
            this.states |= value;
        }

        internal void RemoveState(ElementStates value)
        {
            this.states &= ~value;
        }

        internal void ClearStates()
        {
            this.states = ElementStates.None;
        }

        internal void ToggleState(ElementStates value)
        {
            this.states ^= value;
        }
    }
}
