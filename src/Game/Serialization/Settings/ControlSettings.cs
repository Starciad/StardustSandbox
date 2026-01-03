using Microsoft.Xna.Framework.Input;

using System;
using System.Xml.Serialization;

namespace StardustSandbox.Serialization.Settings
{
    [Serializable]
    [XmlRoot("ControlSettings")]
    public readonly struct ControlSettings : ISettingsModule
    {
        [XmlElement("MoveCameraUp", typeof(Keys))]
        public readonly Keys MoveCameraUp { get; init; }

        [XmlElement("MoveCameraRight", typeof(Keys))]
        public readonly Keys MoveCameraRight { get; init; }

        [XmlElement("MoveCameraDown", typeof(Keys))]
        public readonly Keys MoveCameraDown { get; init; }

        [XmlElement("MoveCameraLeft", typeof(Keys))]
        public readonly Keys MoveCameraLeft { get; init; }

        [XmlElement("TogglePause", typeof(Keys))]
        public readonly Keys TogglePause { get; init; }

        [XmlElement("ClearWorld", typeof(Keys))]
        public readonly Keys ClearWorld { get; init; }

        [XmlElement("Screenshot", typeof(Keys))]
        public readonly Keys Screenshot { get; init; }

        [XmlElement("GenerateWorld", typeof(Keys))]
        public readonly Keys GenerateWorld { get; init; }

        public ControlSettings()
        {
            this.MoveCameraUp = Keys.W;
            this.MoveCameraLeft = Keys.A;
            this.MoveCameraDown = Keys.S;
            this.MoveCameraRight = Keys.D;

            this.TogglePause = Keys.Space;
            this.ClearWorld = Keys.R;

            this.Screenshot = Keys.F12;
            this.GenerateWorld = Keys.G;
        }
    }
}
