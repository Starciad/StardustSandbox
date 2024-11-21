using MessagePack;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Mathematics.Primitives;

using System.Windows.Forms;

namespace StardustSandbox.Core.Models.Settings
{
    [MessagePackObject]
    public struct SGraphicsSettings
    {
        [Key(0)]
        public int ScreenWidth { get; set; }

        [Key(1)]
        public int ScreenHeight { get; set; }

        [Key(2)]
        public bool Resizable { get; set; }

        [Key(3)]
        public bool FullScreen { get; set; }

        [Key(4)]
        public float Framerate { get; set; }

        [Key(5)]
        public bool VSync { get; set; }

        [Key(6)]
        public bool Borderless { get; set; }

        public SGraphicsSettings()
        {
            SSize2 monitorResolution = GetMonitorResolution();
            SSize2 autoResolution = GetAutoResolution(monitorResolution);

            this.ScreenWidth = (int)autoResolution.Width;
            this.ScreenHeight = (int)autoResolution.Height;
            this.Resizable = true;
            this.FullScreen = false;
            this.Framerate = 60f;
            this.VSync = false;
            this.Borderless = false;
        }

        private static SSize2 GetAutoResolution(SSize2 monitorResolution)
        {
            for (int i = SScreenConstants.RESOLUTIONS.Length - 1; i >= 0; i--)
            {
                SSize2 resolution = SScreenConstants.RESOLUTIONS[i];

                if (resolution.Width <= monitorResolution.Width && resolution.Height <= monitorResolution.Height)
                {
                    return resolution;
                }
            }

            return SScreenConstants.RESOLUTIONS[0];
        }

        private static SSize2 GetMonitorResolution()
        {
            int width = Screen.PrimaryScreen.Bounds.Width;
            int height = Screen.PrimaryScreen.Bounds.Height;
            return new SSize2(width, height);
        }
    }
}
