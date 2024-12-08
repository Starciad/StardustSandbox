using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.Core.Constants
{
    public static class SScreenConstants
    {
        public static SSize2 DEFAULT_SCREEN_SIZE => new(DEFAULT_SCREEN_WIDTH, DEFAULT_SCREEN_HEIGHT);
        public static int DEFAULT_SCREEN_WIDTH => RESOLUTIONS[3].Width;
        public static int DEFAULT_SCREEN_HEIGHT => RESOLUTIONS[3].Height;
        public static byte DEFAULT_FRAME_RATE => 60;

        // 16:9 Aspect Ratio
        public static SSize2[] RESOLUTIONS =>
        [
            new SSize2(0640, 0360), // [00] - nHD
            new SSize2(0854, 0480), // [01] - FWVGA
            new SSize2(0960, 0540), // [02] - qHD
            new SSize2(1280, 0720), // [03] - SD / HD ready (720p) [DEFAULT]
            new SSize2(1366, 0768), // [04] - WXGA
            new SSize2(1600, 0900), // [05] - HD+
            new SSize2(1920, 1080), // [06] - FHD / Full HD (1080p)
        ];
    }
}
