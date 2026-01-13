/*
 * Copyright (C) 2026  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;

using StardustSandbox.Databases;
using StardustSandbox.Elements;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Extensions;
using StardustSandbox.Mathematics;

namespace StardustSandbox.WorldSystem
{
    public sealed class SlotLayer
    {
        internal bool IsEmpty => this.elementIndex is ElementIndex.None;
        internal bool HasStoredElement => this.storedElementIndex is not ElementIndex.None;

        internal Element Element => ElementDatabase.GetElement(this.elementIndex);
        internal Element StoredElement => ElementDatabase.GetElement(this.storedElementIndex);

        internal ElementIndex ElementIndex => this.elementIndex;
        internal ElementIndex StoredElementIndex => this.storedElementIndex;
        internal ElementStates States => this.states;
        internal float Temperature => this.temperature;
        internal Color ColorModifier => this.colorModifier;
        internal UpdateCycleFlag StepCycleFlag => this.stepCycleFlag;

        private ElementIndex elementIndex;
        private ElementIndex storedElementIndex;
        private ElementStates states;
        private float temperature;
        private Color colorModifier;
        private UpdateCycleFlag stepCycleFlag;

        internal SlotLayer()
        {
            Reset();
        }

        internal void Instantiate(in ElementIndex index)
        {
            ClearStates();

            this.elementIndex = index;
            this.storedElementIndex = ElementIndex.None;

            this.temperature = this.Element.DefaultTemperature;
            this.colorModifier = Color.White;
            this.stepCycleFlag = UpdateCycleFlag.None;
        }

        internal void Destroy()
        {
            ClearStates();

            this.elementIndex = ElementIndex.None;
            this.storedElementIndex = ElementIndex.None;

            this.temperature = 0;
            this.colorModifier = Color.White;
            this.stepCycleFlag = UpdateCycleFlag.None;
        }

        internal void Copy(in SlotLayer valueToCopy)
        {
            this.elementIndex = valueToCopy.elementIndex;
            this.storedElementIndex = valueToCopy.storedElementIndex;
            this.states = valueToCopy.states;
            this.temperature = valueToCopy.temperature;
            this.colorModifier = valueToCopy.colorModifier;
            this.stepCycleFlag = valueToCopy.stepCycleFlag;
        }

        internal void SetTemperatureValue(in float value)
        {
            this.temperature = TemperatureMath.Clamp(value);
        }

        internal void SetColorModifier(in Color value)
        {
            this.colorModifier = value;
        }

        internal void NextStepCycle()
        {
            this.stepCycleFlag = this.stepCycleFlag.GetNextCycle();
        }

        internal void SetStoredElement(in ElementIndex index)
        {
            this.storedElementIndex = index;
        }

        public void Reset()
        {
            Destroy();
        }

        internal bool HasState(in ElementStates value)
        {
            return this.states.HasFlag(value);
        }

        internal void SetState(in ElementStates value)
        {
            this.states |= value;
        }

        internal void RemoveState(in ElementStates value)
        {
            this.states &= ~value;
        }

        internal void ClearStates()
        {
            this.states = ElementStates.None;
        }

        internal void ToggleState(in ElementStates value)
        {
            this.states ^= value;
        }
    }
}

