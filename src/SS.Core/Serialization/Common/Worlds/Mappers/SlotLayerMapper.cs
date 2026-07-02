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

using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Interfaces.Serialization.Morph;
using StardustSandbox.Core.Serialization.Common.Worlds.Data.V1;
using StardustSandbox.Core.Serialization.Common.Worlds.StorageModels;

namespace StardustSandbox.Core.Serialization.Common.Worlds.Mappers
{
    internal sealed class SlotLayerMapper : IMapper
    {
        public IData ToData(IStorageModel value)
        {
            SlotLayerStorageModel storageModel = (SlotLayerStorageModel)value;

            return new SlotLayerData()
            {
                ColorModifierR = storageModel.ColorModifier.R,
                ColorModifierG = storageModel.ColorModifier.G,
                ColorModifierB = storageModel.ColorModifier.B,
                ColorModifierA = storageModel.ColorModifier.A,
                ElementIndex = (byte)storageModel.ElementIndex,
                IsDissipating = storageModel.IsDissipating,
                IsFalling = storageModel.IsFalling,
                StepCycleFlag = (byte)storageModel.StepCycleFlag,
                StoredElementIndex = (byte)storageModel.StoredElementIndex,
                Temperature = storageModel.Temperature,
                WasPushed = storageModel.WasPushed
            };
        }

        public IStorageModel ToStorageModel(IData value)
        {
            SlotLayerData data = (SlotLayerData)value;

            return new SlotLayerStorageModel()
            {
                ColorModifier = new(data.ColorModifierR, data.ColorModifierG, data.ColorModifierB, data.ColorModifierA),
                ElementIndex = (ElementIndex)data.ElementIndex,
                IsDissipating = data.IsDissipating,
                IsFalling = data.IsFalling,
                StepCycleFlag = (UpdateCycleFlag)data.StepCycleFlag,
                StoredElementIndex = (ElementIndex)data.StoredElementIndex,
                Temperature = data.Temperature,
                WasPushed = data.WasPushed
            };
        }
    }
}
