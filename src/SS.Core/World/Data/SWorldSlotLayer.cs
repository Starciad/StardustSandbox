using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Helpers;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Mathematics;

namespace StardustSandbox.Core.World.Data
{
    public sealed class SWorldSlotLayer
    {
        public ISElement Element => this.element;
        public bool IsEmpty => this.isEmpty;
        public short Temperature => this.temperature;
        public bool FreeFalling => this.freeFalling;
        public Color ColorModifier => this.colorModifier;
        public SUpdateCycleFlag UpdateCycleFlag => this.updateCycleFlag;
        public SUpdateCycleFlag StepCycleFlag => this.stepCycleFlag;

        private ISElement element;
        private bool isEmpty;
        private short temperature;
        private bool freeFalling;
        private Color colorModifier;
        private SUpdateCycleFlag updateCycleFlag;
        private SUpdateCycleFlag stepCycleFlag;

        internal SWorldSlotLayer()
        {
            Reset();
        }

        public void Instantiate(ISElement value)
        {
            this.isEmpty = false;
            this.element = value;
            this.temperature = value.DefaultTemperature;
            this.freeFalling = false;
            this.colorModifier = Color.White;
            this.updateCycleFlag = SUpdateCycleFlag.None;
            this.stepCycleFlag = SUpdateCycleFlag.None;
        }

        public void Destroy()
        {
            this.isEmpty = true;
            this.element = null;
            this.temperature = 0;
            this.freeFalling = false;
            this.colorModifier = Color.White;
            this.updateCycleFlag = SUpdateCycleFlag.None;
            this.stepCycleFlag = SUpdateCycleFlag.None;
        }

        public void Copy(SWorldSlotLayer valueToCopy)
        {
            this.element = valueToCopy.Element;
            this.isEmpty = valueToCopy.IsEmpty;
            this.temperature = valueToCopy.Temperature;
            this.freeFalling = valueToCopy.FreeFalling;
            this.colorModifier = valueToCopy.ColorModifier;
            this.updateCycleFlag = valueToCopy.UpdateCycleFlag;
            this.stepCycleFlag = valueToCopy.StepCycleFlag;
        }

        public void SetTemperatureValue(short value)
        {
            this.temperature = STemperatureMath.Clamp(value);
        }

        public void SetFreeFalling(bool value)
        {
            this.freeFalling = value;
        }

        public void SetColorModifier(Color value)
        {
            this.colorModifier = value;
        }

        public void NextUpdateCycle()
        {
            this.updateCycleFlag = this.updateCycleFlag.GetNextCycle();
        }

        public void NextStepCycle()
        {
            this.stepCycleFlag = this.stepCycleFlag.GetNextCycle();
        }

        public void Reset()
        {
            Destroy();
        }
    }
}
