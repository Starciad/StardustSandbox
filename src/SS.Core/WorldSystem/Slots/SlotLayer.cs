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
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Mathematics;

namespace StardustSandbox.Core.WorldSystem.Slots
{
    internal sealed class SlotLayer
    {
        internal bool HasElement => this.ElementIndex is not ElementIndex.None;
        internal bool HasStoredElement => this.HasElement && this.StoredElementIndex is not ElementIndex.None;

        internal Element Element => this.elementDatabase.GetElement(this.elementIndex);
        internal Element StoredElement => this.elementDatabase.GetElement(this.storedElementIndex);

        public bool IsFalling
        {
            get => this.isFalling;
            set => this.isFalling = value;
        }
        public bool IsDissipating
        {
            get => this.isDissipating;
            set => this.isDissipating = value;
        }
        public bool WasPushed
        {
            get => this.wasPushed;
            set => this.wasPushed = value;
        }
        internal Color ColorModifier
        {
            get => this.colorModifier;
            set => this.colorModifier = value;
        }
        internal ElementIndex ElementIndex
        {
            get => this.elementIndex;
            set => this.elementIndex = value;
        }
        internal ElementIndex StoredElementIndex
        {
            get => this.storedElementIndex;
            set => this.storedElementIndex = value;
        }
        internal float Temperature
        {
            get => this.temperature;
            set => this.temperature = TemperatureMath.Clamp(value);
        }
        internal UpdateCycleFlag StepCycleFlag
        {
            get => this.stepCycleFlag;
            set => this.stepCycleFlag = value;
        }

        private bool isDissipating;
        private bool isFalling;
        private bool wasPushed;
        private Color colorModifier;
        private ElementIndex elementIndex;
        private ElementIndex storedElementIndex;
        private float temperature;
        private UpdateCycleFlag stepCycleFlag;

        private readonly ElementDatabase elementDatabase;

        internal SlotLayer(ElementDatabase elementDatabase)
        {
            this.elementDatabase = elementDatabase;

            this.isDissipating = false;
            this.isFalling = false;
            this.wasPushed = false;
            this.colorModifier = Color.White;
            this.elementIndex = ElementIndex.None;
            this.storedElementIndex = ElementIndex.None;
            this.temperature = 0.0f;
            this.stepCycleFlag = UpdateCycleFlag.None;
        }

        internal void Instantiate(ElementIndex index)
        {
            this.isDissipating = false;
            this.isFalling = false;
            this.wasPushed = false;
            this.colorModifier = Color.White;
            this.elementIndex = index;
            this.storedElementIndex = ElementIndex.None;
            this.temperature = this.Element.InitialTemperature;
            this.stepCycleFlag = UpdateCycleFlag.None;
        }

        internal void Instantiate(SlotLayer valueToCopy)
        {
            this.isDissipating = valueToCopy.isDissipating;
            this.isFalling = valueToCopy.isFalling;
            this.wasPushed = valueToCopy.wasPushed;
            this.colorModifier = valueToCopy.colorModifier;
            this.elementIndex = valueToCopy.elementIndex;
            this.storedElementIndex = valueToCopy.storedElementIndex;
            this.temperature = valueToCopy.temperature;
            this.stepCycleFlag = valueToCopy.stepCycleFlag;
        }

        internal void Destroy()
        {
            this.isDissipating = false;
            this.isFalling = false;
            this.wasPushed = false;
            this.colorModifier = Color.White;
            this.elementIndex = ElementIndex.None;
            this.storedElementIndex = ElementIndex.None;
            this.temperature = 0.0f;
            this.stepCycleFlag = UpdateCycleFlag.None;
        }

        internal void Reset()
        {
            Destroy();
        }
    }
}

