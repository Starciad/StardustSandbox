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

using Microsoft.Xna.Framework.Input;

using StardustSandbox.Core.Interfaces.Serialization.Morph;

namespace StardustSandbox.Core.Serialization.Common.Settings.StorageModels
{
    public sealed class ControlStorageModel : IStorageModel
    {
        public Keys MoveCameraUpKeyboardBinding { get; set; }
        public Keys MoveCameraRightKeyboardBinding { get; set; }
        public Keys MoveCameraDownKeyboardBinding { get; set; }
        public Keys MoveCameraLeftKeyboardBinding { get; set; }
        public Keys MoveCameraFastKeyboardBinding { get; set; }
        public Keys ZoomCameraInKeyboardBinding { get; set; }
        public Keys ZoomCameraOutKeyboardBinding { get; set; }
        public Keys TogglePauseKeyboardBinding { get; set; }
        public Keys ClearWorldKeyboardBinding { get; set; }
        public Keys NextShapeKeyboardBinding { get; set; }
        public Keys ScreenshotKeyboardBinding { get; set; }
        public Keys ToggleFullscreenKeyboardBinding { get; set; }

        public ControlStorageModel()
        {
            this.MoveCameraUpKeyboardBinding = Keys.W;
            this.MoveCameraLeftKeyboardBinding = Keys.A;
            this.MoveCameraDownKeyboardBinding = Keys.S;
            this.MoveCameraRightKeyboardBinding = Keys.D;
            this.MoveCameraFastKeyboardBinding = Keys.LeftShift;
            this.ZoomCameraInKeyboardBinding = Keys.E;
            this.ZoomCameraOutKeyboardBinding = Keys.Q;

            this.TogglePauseKeyboardBinding = Keys.Space;
            this.ClearWorldKeyboardBinding = Keys.C;
            this.NextShapeKeyboardBinding = Keys.Tab;

            this.ScreenshotKeyboardBinding = Keys.F9;
            this.ToggleFullscreenKeyboardBinding = Keys.F11;
        }
    }
}

