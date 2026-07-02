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

using System;
using System.Xml.Serialization;

namespace StardustSandbox.Core.Serialization.Common.Settings.Data.V1
{
    [Serializable]
    [XmlRoot("ControlData")]
    public sealed class ControlData : IData
    {
        [XmlElement("ClearWorldKeyboardBinding", typeof(Keys))]
        public Keys ClearWorldKeyboardBinding { get; set; }

        [XmlElement("MoveCameraDownKeyboardBinding", typeof(Keys))]
        public Keys MoveCameraDownKeyboardBinding { get; set; }

        [XmlElement("MoveCameraFastKeyboardBinding", typeof(Keys))]
        public Keys MoveCameraFastKeyboardBinding { get; set; }

        [XmlElement("MoveCameraLeftKeyboardBinding", typeof(Keys))]
        public Keys MoveCameraLeftKeyboardBinding { get; set; }

        [XmlElement("MoveCameraRightKeyboardBinding", typeof(Keys))]
        public Keys MoveCameraRightKeyboardBinding { get; set; }

        [XmlElement("MoveCameraUpKeyboardBinding", typeof(Keys))]
        public Keys MoveCameraUpKeyboardBinding { get; set; }

        [XmlElement("NextShapeKeyboardBinding", typeof(Keys))]
        public Keys NextShapeKeyboardBinding { get; set; }

        [XmlElement("ScreenshotKeyboardBinding", typeof(Keys))]
        public Keys ScreenshotKeyboardBinding { get; set; }

        [XmlElement("ToggleFullscreenKeyboardBinding", typeof(Keys))]
        public Keys ToggleFullscreenKeyboardBinding { get; set; }

        [XmlElement("TogglePauseKeyboardBinding", typeof(Keys))]
        public Keys TogglePauseKeyboardBinding { get; set; }

        [XmlElement("ZoomCameraInKeyboardBinding", typeof(Keys))]
        public Keys ZoomCameraInKeyboardBinding { get; set; }

        [XmlElement("ZoomCameraOutKeyboardBinding", typeof(Keys))]
        public Keys ZoomCameraOutKeyboardBinding { get; set; }
    }
}

