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
        #region Camera Controls

        [XmlElement("MoveCameraUp", typeof(Keys))]
        public Keys MoveCameraUp { get; set; }

        [XmlElement("MoveCameraRight", typeof(Keys))]
        public Keys MoveCameraRight { get; set; }

        [XmlElement("MoveCameraDown", typeof(Keys))]
        public Keys MoveCameraDown { get; set; }

        [XmlElement("MoveCameraLeft", typeof(Keys))]
        public Keys MoveCameraLeft { get; set; }

        [XmlElement("MoveCameraFast", typeof(Keys))]
        public Keys MoveCameraFast { get; set; }

        #endregion

        #region World Controls

        [XmlElement("TogglePause", typeof(Keys))]
        public Keys TogglePause { get; set; }

        [XmlElement("ClearWorld", typeof(Keys))]
        public Keys ClearWorld { get; set; }

        [XmlElement("NextShape", typeof(Keys))]
        public Keys NextShape { get; set; }

        #endregion

        #region Tool Controls

        [XmlElement("Screenshot", typeof(Keys))]
        public Keys Screenshot { get; set; }

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

            this.Screenshot = Keys.F9;
        }
    }
}

