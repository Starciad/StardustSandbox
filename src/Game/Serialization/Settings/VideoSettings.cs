using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Mathematics.Primitives;

using System.Xml.Serialization;

namespace StardustSandbox.Serialization.Settings
{
    [XmlRoot("VideoSettings")]
    public sealed class VideoSettings : SettingsModule
    {
        [XmlElement("Framerate", typeof(float))]
        public float Framerate { get; set; }

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
        public Resolution Resolution
        {
            get => new(this.Width, this.Height);

            set
            {
                this.Width = value.Width;
                this.Height = value.Height;
            }
        }

        public VideoSettings()
        {
            this.Framerate = ScreenConstants.FRAMERATE;
            this.Width = 0;
            this.Height = 0;
            this.FullScreen = false;
            this.VSync = false;
            this.Borderless = false;
        }

        public void UpdateResolution(GraphicsDevice graphicsDevice)
        {
            Resolution monitorResolution = new(
                graphicsDevice.Adapter.CurrentDisplayMode.Width,
                graphicsDevice.Adapter.CurrentDisplayMode.Height
            );

            Resolution autoResolution = GetAutoResolution(monitorResolution);

            this.Width = autoResolution.Width;
            this.Height = autoResolution.Height;
        }

        private static Resolution GetAutoResolution(Resolution monitorResolution)
        {
            for (int i = ScreenConstants.RESOLUTIONS.Length - 1; i >= 0; i--)
            {
                Resolution resolution = ScreenConstants.RESOLUTIONS[i];

                if (resolution.Width <= monitorResolution.Width && resolution.Height <= monitorResolution.Height)
                {
                    return resolution;
                }
            }

            return ScreenConstants.RESOLUTIONS[0];
        }
    }
}
