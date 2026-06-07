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
using StardustSandbox.Core.Interfaces.Serialization.Migrations;
using StardustSandbox.Core.WorldSystem.Slots;

namespace StardustSandbox.Core.Serialization.Worlds.StorageModels
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

        internal SlotLayerStorageModel(SlotLayer slotLayer)
        {
            this.ColorModifier = slotLayer.ColorModifier;
            this.ElementIndex = slotLayer.ElementIndex;
            this.StepCycleFlag = slotLayer.StepCycleFlag;
            this.StoredElementIndex = slotLayer.StoredElementIndex;
            this.Temperature = slotLayer.Temperature;
            this.IsFalling = slotLayer.IsFalling;
            this.WasPushed = slotLayer.WasPushed;
            this.IsDissipating = slotLayer.IsDissipating;
        }
    }
}

