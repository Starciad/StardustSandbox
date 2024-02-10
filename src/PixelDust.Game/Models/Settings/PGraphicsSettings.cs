using MessagePack;

using PixelDust.Game.Constants;
using PixelDust.Game.Mathematics;

namespace PixelDust.Game.Models.Settings
{
    [MessagePackObject]
    public struct PGraphicsSettings
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

        public PGraphicsSettings()
        {
            Size2Int resolution = PScreenConstants.RESOLUTIONS[3];

            ScreenWidth = resolution.Width;
            ScreenHeight = resolution.Height;
            Resizable = false;
            FullScreen = false;
            Framerate = 60f;
            VSync = false;
            Borderless = false;
        }
    }
}
