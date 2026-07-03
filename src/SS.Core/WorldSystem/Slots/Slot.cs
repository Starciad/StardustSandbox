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
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.Collections;

namespace StardustSandbox.Core.WorldSystem.Slots
{
    internal sealed class Slot : IPoolableObject
    {
        internal Point Position { get; set; }

        private readonly SlotLayer foreground;
        private readonly SlotLayer background;

        internal Slot(ElementDatabase elementDatabase)
        {
            this.foreground = new(elementDatabase);
            this.background = new(elementDatabase);
        }

        private SlotLayer GetLayer(Layer layer)
        {
            return layer switch
            {
                Layer.Foreground => this.foreground,
                Layer.Background => this.background,
                _ => null,
            };
        }

        #region Handling

        internal void Destroy(Layer layer)
        {
            GetLayer(layer).Destroy();
        }

        internal void Instantiate(Layer layer, ElementIndex index)
        {
            GetLayer(layer).Instantiate(index);
        }

        internal void Instantiate(Layer layer, Slot valueToCopy)
        {
            GetLayer(layer).Instantiate(valueToCopy.GetLayer(layer));
        }

        internal void Reset(Layer layer)
        {
            GetLayer(layer).Reset();
        }

        public void Reset()
        {
            this.foreground.Reset();
            this.background.Reset();
        }

        #endregion

        #region Getters

        internal bool GetDissipatingState(Layer layer)
        {
            return GetLayer(layer).IsDissipating;
        }

        internal bool GetFallingState(Layer layer)
        {
            return GetLayer(layer).IsFalling;
        }

        internal bool GetPushedState(Layer layer)
        {
            return GetLayer(layer).WasPushed;
        }

        internal Color GetColorModifier(Layer layer)
        {
            return GetLayer(layer).ColorModifier;
        }

        internal Element GetElement(Layer layer)
        {
            return GetLayer(layer).Element;
        }

        internal ElementIndex GetElementIndex(Layer layer)
        {
            return GetLayer(layer).ElementIndex;
        }

        internal Element GetStoredElement(Layer layer)
        {
            return GetLayer(layer).StoredElement;
        }

        internal ElementIndex GetStoredElementIndex(Layer layer)
        {
            return GetLayer(layer).StoredElementIndex;
        }

        internal float GetTemperature(Layer layer)
        {
            return GetLayer(layer).Temperature;
        }

        internal UpdateCycleFlag GetStepCycleFlag(Layer layer)
        {
            return GetLayer(layer).StepCycleFlag;
        }

        internal bool HasElement(Layer layer)
        {
            return GetLayer(layer).HasElement;
        }

        internal bool HasStoredElement(Layer layer)
        {
            return GetLayer(layer).HasStoredElement;
        }

        #endregion

        #region Setters

        internal void SetDissipatingState(Layer layer, bool value)
        {
            GetLayer(layer).IsDissipating = value;
        }

        internal void SetFallingState(Layer layer, bool value)
        {
            GetLayer(layer).IsFalling = value;
        }

        internal void SetPushedState(Layer layer, bool value)
        {
            GetLayer(layer).WasPushed = value;
        }

        internal void SetColorModifier(Layer layer, Color value)
        {
            GetLayer(layer).ColorModifier = value;
        }

        internal void SetElementIndex(Layer layer, ElementIndex value)
        {
            GetLayer(layer).ElementIndex = value;
        }

        internal void SetStoredElementIndex(Layer layer, ElementIndex value)
        {
            GetLayer(layer).StoredElementIndex = value;
        }

        internal void SetTemperature(Layer layer, float value)
        {
            GetLayer(layer).Temperature = value;
        }

        internal void SetStepCycleFlag(Layer layer, UpdateCycleFlag value)
        {
            GetLayer(layer).StepCycleFlag = value;
        }

        #endregion
    }
}
