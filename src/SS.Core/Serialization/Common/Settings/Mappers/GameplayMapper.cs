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

using StardustSandbox.Core.Interfaces.Serialization.Morph;
using StardustSandbox.Core.Serialization.Common.Settings.Data.V1;
using StardustSandbox.Core.Serialization.Common.Settings.StorageModels;

namespace StardustSandbox.Core.Serialization.Common.Settings.Mappers
{
    internal sealed class GameplayMapper : IMapper
    {
        public IData ToData(IStorageModel value)
        {
            GameplayStorageModel storageModel = (GameplayStorageModel)value;

            return new GameplayData()
            {
                EnableSmoothCameraMovement = storageModel.EnableSmoothCameraMovement,
                GridOpacity = storageModel.GridOpacity,
                PreviewAreaColorA = storageModel.PreviewAreaColor.A,
                PreviewAreaColorB = storageModel.PreviewAreaColor.B,
                PreviewAreaColorG = storageModel.PreviewAreaColor.G,
                PreviewAreaColorR = storageModel.PreviewAreaColor.R,
                ShowGrid = storageModel.ShowGrid,
                ShowPreviewArea = storageModel.ShowPreviewArea,
                ShowTemperatureColorVariations = storageModel.ShowTemperatureColorVariations,
            };
        }

        public IStorageModel ToStorageModel(IData value)
        {
            GameplayData data = (GameplayData)value;

            return new GameplayStorageModel()
            {
                EnableSmoothCameraMovement = data.EnableSmoothCameraMovement,
                GridOpacity = data.GridOpacity,
                PreviewAreaColor = new(data.PreviewAreaColorR, data.PreviewAreaColorG, data.PreviewAreaColorB, data.PreviewAreaColorA),
                ShowGrid = data.ShowGrid,
                ShowPreviewArea = data.ShowPreviewArea,
                ShowTemperatureColorVariations = data.ShowTemperatureColorVariations,
            };
        }
    }
}
