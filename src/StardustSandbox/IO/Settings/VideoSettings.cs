using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;

using System.Xml.Serialization;

namespace StardustSandbox.IO.Settings
{
    [XmlRoot("VideoSettings")]
    public sealed class VideoSettings : SettingsModule
    {
        [XmlElement("Width", typeof(int))]
        public int Width { get; set; }

        [XmlElement("Height", typeof(int))]
        public int Height { get; set; }

        [XmlElement("FullScreen", typeof(bool))]
        public bool FullScreen { get; set; }

        [XmlElement("VSync", typeof(bool))]
        public bool VSync { get; set; }

        [XmlElement("Borderless", typeof(bool))]
        public bool Borderless { get; set; }

        [XmlIgnore]
        public Point Resolution
        {
            get => new(this.Width, this.Height);

            set
            {
                this.Width = value.X;
                this.Height = value.Y;
            }
        }

        public VideoSettings()
        {
            this.Width = 0;
            this.Height = 0;
            this.FullScreen = false;
            this.VSync = false;
            this.Borderless = false;
        }

        public void UpdateResolution(GraphicsDevice graphicsDevice)
        {
            Point monitorResolution = new(
                graphicsDevice.Adapter.CurrentDisplayMode.Width,
                graphicsDevice.Adapter.CurrentDisplayMode.Height
            );

            Point autoResolution = GetAutoResolution(monitorResolution);

            this.Width = autoResolution.X;
            this.Height = autoResolution.Y;
        }

        private static Point GetAutoResolution(Point monitorResolution)
        {
            for (int i = ScreenConstants.RESOLUTIONS.Length - 1; i >= 0; i--)
            {
                Point resolution = ScreenConstants.RESOLUTIONS[i];

                if (resolution.X <= monitorResolution.X && resolution.Y <= monitorResolution.Y)
                {
                    return resolution;
                }
            }

            return ScreenConstants.RESOLUTIONS[0];
        }
    }
}
