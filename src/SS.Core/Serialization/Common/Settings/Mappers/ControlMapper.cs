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
    internal sealed class ControlMapper : IMapper
    {
        public IData ToData(IStorageModel value)
        {
            ControlStorageModel storageModel = (ControlStorageModel)value;

            return new ControlData()
            {
                ClearWorldKeyboardBinding = storageModel.ClearWorldKeyboardBinding,
                MoveCameraDownKeyboardBinding = storageModel.MoveCameraDownKeyboardBinding,
                MoveCameraFastKeyboardBinding = storageModel.MoveCameraFastKeyboardBinding,
                MoveCameraLeftKeyboardBinding = storageModel.MoveCameraLeftKeyboardBinding,
                MoveCameraRightKeyboardBinding = storageModel.MoveCameraRightKeyboardBinding,
                MoveCameraUpKeyboardBinding = storageModel.MoveCameraUpKeyboardBinding,
                NextShapeKeyboardBinding = storageModel.NextShapeKeyboardBinding,
                ScreenshotKeyboardBinding = storageModel.ScreenshotKeyboardBinding,
                ToggleFullscreenKeyboardBinding = storageModel.ToggleFullscreenKeyboardBinding,
                TogglePauseKeyboardBinding = storageModel.TogglePauseKeyboardBinding,
                ZoomCameraInKeyboardBinding = storageModel.ZoomCameraInKeyboardBinding,
                ZoomCameraOutKeyboardBinding = storageModel.ZoomCameraOutKeyboardBinding
            };
        }

        public IStorageModel ToStorageModel(IData value)
        {
            ControlData data = (ControlData)value;

            return new ControlStorageModel()
            {
                ClearWorldKeyboardBinding = data.ClearWorldKeyboardBinding,
                MoveCameraDownKeyboardBinding = data.MoveCameraDownKeyboardBinding,
                MoveCameraFastKeyboardBinding = data.MoveCameraFastKeyboardBinding,
                MoveCameraLeftKeyboardBinding = data.MoveCameraLeftKeyboardBinding,
                MoveCameraRightKeyboardBinding = data.MoveCameraRightKeyboardBinding,
                MoveCameraUpKeyboardBinding = data.MoveCameraUpKeyboardBinding,
                NextShapeKeyboardBinding = data.NextShapeKeyboardBinding,
                ScreenshotKeyboardBinding = data.ScreenshotKeyboardBinding,
                ToggleFullscreenKeyboardBinding = data.ToggleFullscreenKeyboardBinding,
                TogglePauseKeyboardBinding = data.TogglePauseKeyboardBinding,
                ZoomCameraInKeyboardBinding = data.ZoomCameraInKeyboardBinding,
                ZoomCameraOutKeyboardBinding = data.ZoomCameraOutKeyboardBinding
            };
        }
    }
}
