using Microsoft.Xna.Framework;

namespace StardustSandbox.Constants
{
    internal static class ScreenConstants
    {
        internal static Point DEFAULT_SCREEN_SIZE => new(DEFAULT_SCREEN_WIDTH, DEFAULT_SCREEN_HEIGHT);

        internal const byte DEFAULT_FRAME_RATE = 60;
        internal const int DEFAULT_SCREEN_WIDTH = 1280;
        internal const int DEFAULT_SCREEN_HEIGHT = 720;

        // 16:9 Aspect Ratio
        internal static Point[] RESOLUTIONS =>
        [
            new Point(0640, 0360), // [00] - nHD
            new Point(0854, 0480), // [01] - FWVGA
            new Point(0960, 0540), // [02] - qHD
            new Point(1280, 0720), // [03] - SD / HD ready (720p) [DEFAULT]
            new Point(1366, 0768), // [04] - WXGA
            new Point(1600, 0900), // [05] - HD+
            new Point(1920, 1080), // [06] - FHD / Full HD (1080p)
        ];
    }
}
