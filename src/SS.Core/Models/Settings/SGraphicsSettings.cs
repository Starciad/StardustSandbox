using MessagePack;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Mathematics.Primitives;

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
            SSize2 resolution = SScreenConstants.RESOLUTIONS[4];

            this.ScreenWidth = (int)resolution.Width;
            this.ScreenHeight = (int)resolution.Height;
            this.Resizable = false;
            this.FullScreen = false;
            this.Framerate = 60f;
            this.VSync = false;
            this.Borderless = false;
        }
    }
}
