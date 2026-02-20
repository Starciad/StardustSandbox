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

using StardustSandbox.Core.Interfaces.Serialization;

using System;
using System.Xml.Serialization;

namespace StardustSandbox.Core.Serialization.Settings
{
    [Serializable]
    [XmlRoot("ControlSettings")]
    public sealed class ControlSettings : ISettingsModule
    {
        [XmlElement("MoveCameraUpKeyboardBinding", typeof(Keys))]
        public Keys MoveCameraUpKeyboardBinding { get; set; }

        [XmlElement("MoveCameraRightKeyboardBinding", typeof(Keys))]
        public Keys MoveCameraRightKeyboardBinding { get; set; }

        [XmlElement("MoveCameraDownKeyboardBinding", typeof(Keys))]
        public Keys MoveCameraDownKeyboardBinding { get; set; }

        [XmlElement("MoveCameraLeftKeyboardBinding", typeof(Keys))]
        public Keys MoveCameraLeftKeyboardBinding { get; set; }

        [XmlElement("MoveCameraFastKeyboardBinding", typeof(Keys))]
        public Keys MoveCameraFastKeyboardBinding { get; set; }

        [XmlElement("ZoomCameraInKeyboardBinding", typeof(Keys))]
        public Keys ZoomCameraInKeyboardBinding { get; set; }

        [XmlElement("ZoomCameraOutKeyboardBinding", typeof(Keys))]
        public Keys ZoomCameraOutKeyboardBinding { get; set; }

        [XmlElement("TogglePauseKeyboardBinding", typeof(Keys))]
        public Keys TogglePauseKeyboardBinding { get; set; }

        [XmlElement("ClearWorldKeyboardBinding", typeof(Keys))]
        public Keys ClearWorldKeyboardBinding { get; set; }

        [XmlElement("NextShapeKeyboardBinding", typeof(Keys))]
        public Keys NextShapeKeyboardBinding { get; set; }

        [XmlElement("ScreenshotKeyboardBinding", typeof(Keys))]
        public Keys ScreenshotKeyboardBinding { get; set; }

        [XmlElement("UINavigateUpKeyboardBinding", typeof(Keys))]
        public Keys UINavigateUpKeyboardBinding { get; set; }

        [XmlElement("UINavigateRightKeyboardBinding", typeof(Keys))]
        public Keys UINavigateRightKeyboardBinding { get; set; }

        [XmlElement("UINavigateDownKeyboardBinding", typeof(Keys))]
        public Keys UINavigateDownKeyboardBinding { get; set; }

        [XmlElement("UINavigateLeftKeyboardBinding", typeof(Keys))]
        public Keys UINavigateLeftKeyboardBinding { get; set; }

        [XmlElement("UISelectKeyboardBinding", typeof(Keys))]
        public Keys UISelectKeyboardBinding { get; set; }

        [XmlElement("MoveCameraUpControllerBinding", typeof(Buttons))]
        public Buttons MoveCameraUpControllerBinding { get; set; }

        [XmlElement("MoveCameraRightControllerBinding", typeof(Buttons))]
        public Buttons MoveCameraRightControllerBinding { get; set; }

        [XmlElement("MoveCameraDownControllerBinding", typeof(Buttons))]
        public Buttons MoveCameraDownControllerBinding { get; set; }

        [XmlElement("MoveCameraLeftControllerBinding", typeof(Buttons))]
        public Buttons MoveCameraLeftControllerBinding { get; set; }

        [XmlElement("MoveCameraFastControllerBinding", typeof(Buttons))]
        public Buttons MoveCameraFastControllerBinding { get; set; }

        [XmlElement("ZoomCameraInControllerBinding", typeof(Buttons))]
        public Buttons ZoomCameraInControllerBinding { get; set; }

        [XmlElement("ZoomCameraOutControllerBinding", typeof(Buttons))]
        public Buttons ZoomCameraOutControllerBinding { get; set; }

        [XmlElement("UINavigateUpControllerBinding", typeof(Buttons))]
        public Buttons UINavigateUpControllerBinding { get; set; }

        [XmlElement("UINavigateRightControllerBinding", typeof(Buttons))]
        public Buttons UINavigateRightControllerBinding { get; set; }

        [XmlElement("UINavigateDownControllerBinding", typeof(Buttons))]
        public Buttons UINavigateDownControllerBinding { get; set; }

        [XmlElement("UINavigateLeftControllerBinding", typeof(Buttons))]
        public Buttons UINavigateLeftControllerBinding { get; set; }

        [XmlElement("UISelectControllerBinding", typeof(Buttons))]
        public Buttons UISelectControllerBinding { get; set; }

        public ControlSettings()
        {
            // Keyboard
            this.MoveCameraUpKeyboardBinding = Keys.W;
            this.MoveCameraLeftKeyboardBinding = Keys.A;
            this.MoveCameraDownKeyboardBinding = Keys.S;
            this.MoveCameraRightKeyboardBinding = Keys.D;
            this.MoveCameraFastKeyboardBinding = Keys.LeftShift;
            this.ZoomCameraInKeyboardBinding = Keys.OemPlus;
            this.ZoomCameraOutKeyboardBinding = Keys.OemMinus;

            this.TogglePauseKeyboardBinding = Keys.Space;
            this.ClearWorldKeyboardBinding = Keys.R;
            this.NextShapeKeyboardBinding = Keys.Tab;

            this.ScreenshotKeyboardBinding = Keys.F9;

            this.UINavigateUpKeyboardBinding = Keys.W;
            this.UINavigateLeftKeyboardBinding = Keys.A;
            this.UINavigateDownKeyboardBinding = Keys.S;
            this.UINavigateRightKeyboardBinding = Keys.D;
            this.UISelectKeyboardBinding = Keys.Enter;

            // Gamepad
            this.MoveCameraUpControllerBinding = Buttons.LeftThumbstickUp;
            this.MoveCameraLeftControllerBinding = Buttons.LeftThumbstickLeft;
            this.MoveCameraDownControllerBinding = Buttons.LeftThumbstickDown;
            this.MoveCameraRightControllerBinding = Buttons.LeftThumbstickRight;
            // this.MoveCameraFastControllerBinding = Buttons.LeftShoulder;
            // this.ZoomCameraInControllerBinding = Buttons.RightTrigger;
            // this.ZoomCameraOutControllerBinding = Buttons.LeftTrigger;

            this.UINavigateUpControllerBinding = Buttons.LeftThumbstickUp;
            this.UINavigateLeftControllerBinding = Buttons.LeftThumbstickLeft;
            this.UINavigateDownControllerBinding = Buttons.LeftThumbstickDown;
            this.UINavigateRightControllerBinding = Buttons.LeftThumbstickRight;
            this.UISelectControllerBinding = Buttons.A;
        }
    }
}

