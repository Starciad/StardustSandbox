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

using System;
using System.Xml.Serialization;

namespace StardustSandbox.Serialization.Settings
{
    [Serializable]
    [XmlRoot("ControlSettings")]
    public readonly struct ControlSettings : ISettingsModule
    {
        #region Camera Controls

        [XmlElement("MoveCameraUp", typeof(Keys))]
        public readonly Keys MoveCameraUp { get; init; }

        [XmlElement("MoveCameraRight", typeof(Keys))]
        public readonly Keys MoveCameraRight { get; init; }

        [XmlElement("MoveCameraDown", typeof(Keys))]
        public readonly Keys MoveCameraDown { get; init; }

        [XmlElement("MoveCameraLeft", typeof(Keys))]
        public readonly Keys MoveCameraLeft { get; init; }

        [XmlElement("MoveCameraFast", typeof(Keys))]
        public readonly Keys MoveCameraFast { get; init; }

        #endregion

        #region World Controls

        [XmlElement("TogglePause", typeof(Keys))]
        public readonly Keys TogglePause { get; init; }

        [XmlElement("ClearWorld", typeof(Keys))]
        public readonly Keys ClearWorld { get; init; }

        [XmlElement("NextShape", typeof(Keys))]
        public readonly Keys NextShape { get; init; }

        #endregion

        #region Tool Controls

        [XmlElement("Screenshot", typeof(Keys))]
        public readonly Keys Screenshot { get; init; }

        #endregion

        public ControlSettings()
        {
            this.MoveCameraUp = Keys.W;
            this.MoveCameraLeft = Keys.A;
            this.MoveCameraDown = Keys.S;
            this.MoveCameraRight = Keys.D;
            this.MoveCameraFast = Keys.LeftShift;

            this.TogglePause = Keys.Space;
            this.ClearWorld = Keys.R;
            this.NextShape = Keys.Tab;

            this.Screenshot = Keys.F12;
        }
    }
}

