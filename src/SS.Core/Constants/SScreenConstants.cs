using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.Core.Constants
{
    public static class SScreenConstants
    {
        public static SSize2 DEFAULT_SCREEN_SIZE => new(DEFAULT_SCREEN_WIDTH, DEFAULT_SCREEN_HEIGHT);
        public static int DEFAULT_SCREEN_WIDTH => RESOLUTIONS[3].Width;
        public static int DEFAULT_SCREEN_HEIGHT => RESOLUTIONS[3].Height;

        // 16:9 Aspect Ratio
        public static SSize2[] RESOLUTIONS =>
        [
            new SSize2(640, 360), // nHD
            new SSize2(854, 480), // FWVGA
            new SSize2(960, 540), // qHD
            new SSize2(1280, 720), // SD / HD ready (720p) [DEFAULT]
            new SSize2(1366, 768), // WXGA
            new SSize2(1600, 900), // HD+
            new SSize2(1920, 1080), // FHD / Full HD (1080p)
            new SSize2(2560, 1440), // WQHD
            new SSize2(3200, 1800), // QHD+
            new SSize2(3840, 2160), // 4K UHD
            new SSize2(5120, 2880), // 5K
            new SSize2(7680, 4320), // 8K UHD
            new SSize2(15360, 8640) // 16K UHD
        ];
    }
}
