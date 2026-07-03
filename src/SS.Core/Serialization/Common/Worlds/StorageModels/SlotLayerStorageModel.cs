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

using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.Serialization.Morph;
using StardustSandbox.Core.WorldSystem.Slots;

namespace StardustSandbox.Core.Serialization.Common.Worlds.StorageModels
{
    internal sealed class SlotLayerStorageModel : IStorageModel
    {
        internal Color ColorModifier { get; set; }
        internal ElementIndex ElementIndex { get; set; }
        internal UpdateCycleFlag StepCycleFlag { get; set; }
        internal ElementIndex StoredElementIndex { get; set; }
        internal float Temperature { get; set; }
        internal bool IsFalling { get; set; }
        internal bool WasPushed { get; set; }
        internal bool IsDissipating { get; set; }

        internal SlotLayerStorageModel()
        {

        }

        internal SlotLayerStorageModel(Slot slot, Layer layer)
        {
            this.ColorModifier = slot.GetColorModifier(layer);
            this.ElementIndex = slot.GetElementIndex(layer);
            this.StepCycleFlag = slot.GetStepCycleFlag(layer);
            this.StoredElementIndex = slot.GetStoredElementIndex(layer);
            this.Temperature = slot.GetTemperature(layer);
            this.IsFalling = slot.GetFallingState(layer);
            this.WasPushed = slot.GetPushedState(layer);
            this.IsDissipating = slot.GetDissipatingState(layer);
        }
    }
}

