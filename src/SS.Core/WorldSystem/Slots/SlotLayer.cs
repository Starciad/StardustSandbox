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

namespace StardustSandbox.Core.WorldSystem.Slots
{
    internal sealed class SlotLayer
    {
        internal bool IsEmpty => this.ElementIndex is ElementIndex.None;
        internal bool HasStoredElement => this.StoredElementIndex is not ElementIndex.None;

        internal Element Element => this.elementDatabase.GetElement(this.ElementIndex);
        internal Element StoredElement => this.elementDatabase.GetElement(this.StoredElementIndex);

        internal Color ColorModifier { get; set; }
        internal ElementIndex ElementIndex { get; set; }
        internal ElementStates States { get; set; }
        internal UpdateCycleFlag StepCycleFlag { get; set; }
        internal ElementIndex StoredElementIndex { get; set; }
        internal float Temperature { get => this.temperature; set => this.temperature = TemperatureMath.Clamp(value); }

        private float temperature;

        private readonly ElementDatabase elementDatabase;

        internal SlotLayer(ElementDatabase elementDatabase)
        {
            this.elementDatabase = elementDatabase;
            Reset();
        }

        #region Lifecycle Management

        internal void Instantiate(ElementIndex index)
        {
            this.States = ElementStates.None;
            this.ColorModifier = Color.White;
            this.ElementIndex = index;
            this.StepCycleFlag = UpdateCycleFlag.None;
            this.StoredElementIndex = ElementIndex.None;
            this.Temperature = this.Element.InitialTemperature;
        }

        internal void Destroy()
        {
            this.States = ElementStates.None;
            this.ColorModifier = Color.White;
            this.ElementIndex = ElementIndex.None;
            this.StepCycleFlag = UpdateCycleFlag.None;
            this.StoredElementIndex = ElementIndex.None;
            this.Temperature = 0;
        }

        internal void Copy(SlotLayer target)
        {
            this.ColorModifier = target.ColorModifier;
            this.ElementIndex = target.ElementIndex;
            this.States = target.States;
            this.StepCycleFlag = target.StepCycleFlag;
            this.StoredElementIndex = target.StoredElementIndex;
            this.Temperature = target.Temperature;
        }

        internal void Reset()
        {
            Destroy();
        }

        #endregion

        #region States Management

        internal bool HasState(ElementStates value)
        {
            return this.States.HasFlag(value);
        }

        internal void RemoveState(ElementStates value)
        {
            this.States &= ~value;
        }

        internal void SetState(ElementStates value)
        {
            this.States |= value;
        }

        internal void ToggleState(ElementStates value)
        {
            this.States ^= value;
        }

        #endregion
    }
}

