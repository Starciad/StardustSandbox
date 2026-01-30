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
using StardustSandbox.Core.Mathematics;

namespace StardustSandbox.Core.WorldSystem
{
    public sealed class SlotLayer
    {
        internal bool IsEmpty => this.ElementIndex is ElementIndex.None;
        internal bool HasStoredElement => this.StoredElementIndex is not Enums.Elements.ElementIndex.None;

        internal Element Element => ElementDatabase.GetElement(this.ElementIndex);
        internal Element StoredElement => ElementDatabase.GetElement(this.StoredElementIndex);

        internal Color ColorModifier { get; set; }
        internal ElementIndex ElementIndex { get; set; }
        internal ElementStates States { get; set; }
        internal UpdateCycleFlag StepCycleFlag { get; set; }
        internal ElementIndex StoredElementIndex { get; set; }
        internal float Temperature { get => this.temperature; set => this.temperature = TemperatureMath.Clamp(value); }
        internal int TicksRemaining { get; set; }

        private float temperature;

        internal SlotLayer()
        {
            Reset();
        }

        // Lifecycle Management
        internal void Instantiate(in ElementIndex index)
        {
            ClearStates();

            this.ColorModifier = Color.White;
            this.ElementIndex = index;
            this.StepCycleFlag = UpdateCycleFlag.None;
            this.StoredElementIndex = Enums.Elements.ElementIndex.None;
            this.Temperature = this.Element.DefaultTemperature;
        }
        internal void Destroy()
        {
            ClearStates();

            this.ColorModifier = Color.White;
            this.ElementIndex = ElementIndex.None;
            this.StepCycleFlag = UpdateCycleFlag.None;
            this.StoredElementIndex = Enums.Elements.ElementIndex.None;
            this.Temperature = 0;
        }
        internal void Copy(in SlotLayer valueToCopy)
        {
            this.ColorModifier = valueToCopy.ColorModifier;
            this.ElementIndex = valueToCopy.ElementIndex;
            this.States = valueToCopy.States;
            this.StepCycleFlag = valueToCopy.StepCycleFlag;
            this.StoredElementIndex = valueToCopy.StoredElementIndex;
            this.Temperature = valueToCopy.Temperature;
        }

        public void Reset()
        {
            Destroy();
        }

        // States Management
        internal void ClearStates()
        {
            this.States = ElementStates.None;
        }
        internal bool HasState(in ElementStates value)
        {
            return this.States.HasFlag(value);
        }
        internal void RemoveState(in ElementStates value)
        {
            this.States &= ~value;
        }
        internal void SetState(in ElementStates value)
        {
            this.States |= value;
        }
        internal void ToggleState(in ElementStates value)
        {
            this.States ^= value;
        }
    }
}

