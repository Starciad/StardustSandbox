/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
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

using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Mathematics;

namespace StardustSandbox.Core.WorldSystem
{
    public sealed class SlotLayer
    {
        internal bool IsEmpty => this.elementIndex is ElementIndex.None;
        internal bool HasStoredElement => this.storedElementIndex is not ElementIndex.None;

        internal Element Element => ElementDatabase.GetElement(this.elementIndex);
        internal Element StoredElement => ElementDatabase.GetElement(this.storedElementIndex);

        internal Color ColorModifier => this.colorModifier;
        internal ElementIndex ElementIndex => this.elementIndex;
        internal ElementStates States => this.states;
        internal UpdateCycleFlag StepCycleFlag => this.stepCycleFlag;
        internal ElementIndex StoredElementIndex => this.storedElementIndex;
        internal float Temperature => this.temperature;

        private Color colorModifier;
        private ElementIndex elementIndex;
        private ElementStates states;
        private UpdateCycleFlag stepCycleFlag;
        private ElementIndex storedElementIndex;
        private float temperature;

        internal SlotLayer()
        {
            Reset();
        }

        // Lifecycle Management
        internal void Instantiate(in ElementIndex index)
        {
            ClearStates();

            this.colorModifier = Color.White;
            this.elementIndex = index;
            this.stepCycleFlag = UpdateCycleFlag.None;
            this.storedElementIndex = ElementIndex.None;
            this.temperature = this.Element.DefaultTemperature;
        }
        internal void Destroy()
        {
            ClearStates();

            this.colorModifier = Color.White;
            this.elementIndex = ElementIndex.None;
            this.stepCycleFlag = UpdateCycleFlag.None;
            this.storedElementIndex = ElementIndex.None;
            this.temperature = 0;
        }
        internal void Copy(in SlotLayer valueToCopy)
        {
            this.colorModifier = valueToCopy.colorModifier;
            this.elementIndex = valueToCopy.elementIndex;
            this.states = valueToCopy.states;
            this.stepCycleFlag = valueToCopy.stepCycleFlag;
            this.storedElementIndex = valueToCopy.storedElementIndex;
            this.temperature = valueToCopy.temperature;
        }

        public void Reset()
        {
            Destroy();
        }

        // Property Management
        internal void NextStepCycle()
        {
            this.stepCycleFlag = this.stepCycleFlag.GetNextCycle();
        }
        internal void SetTemperatureValue(in float value)
        {
            this.temperature = TemperatureMath.Clamp(value);
        }
        internal void SetColorModifier(in Color value)
        {
            this.colorModifier = value;
        }
        internal void SetStoredElement(in ElementIndex index)
        {
            this.storedElementIndex = index;
        }

        // States Management
        internal void ClearStates()
        {
            this.states = ElementStates.None;
        }
        internal bool HasState(in ElementStates value)
        {
            return this.states.HasFlag(value);
        }
        internal void RemoveState(in ElementStates value)
        {
            this.states &= ~value;
        }
        internal void SetState(in ElementStates value)
        {
            this.states |= value;
        }
        internal void ToggleState(in ElementStates value)
        {
            this.states ^= value;
        }
    }
}

