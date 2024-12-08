using MessagePack;

using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.Core.IO.Files.Settings
{
    [MessagePackObject]
    public sealed class SVideoSettings : SSettings
    {
        [IgnoreMember]
        public SSize2 Resolution
        {
            get
            {
                return new(this.ScreenWidth, this.ScreenHeight);
            }

            set
            {
                this.ScreenWidth = (ushort)value.Width;
                this.ScreenHeight = (ushort)value.Height;
            }
        }

        [Key(0)]
        public ushort ScreenWidth { get; set; }

        [Key(1)]
        public ushort ScreenHeight { get; set; }

        [Key(2)]
        public bool FullScreen { get; set; }

        [Key(3)]
        public bool VSync { get; set; }

        [Key(4)]
        public bool Borderless { get; set; }

        public SVideoSettings()
        {
            SSize2 monitorResolution = GetMonitorResolution();
            SSize2 autoResolution = GetAutoResolution(monitorResolution);

            this.ScreenWidth = (ushort)autoResolution.Width;
            this.ScreenHeight = (ushort)autoResolution.Height;
            this.FullScreen = false;
            this.VSync = false;
            this.Borderless = false;
        }

        private static SSize2 GetMonitorResolution()
        {
            int width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            return new SSize2(width, height);
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
    }
}
