using Microsoft.Xna.Framework;

namespace StardustSandbox.Constants
{
    internal static class ScreenConstants
    {
        internal const double FRAMERATE = 60.0;
        internal const int SCREEN_WIDTH = 1280;
        internal const int SCREEN_HEIGHT = 720;

        internal static Point SCREEN_DIMENSIONS => new(SCREEN_WIDTH, SCREEN_HEIGHT);

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

        internal static double[] FRAMERATES =>
        [
            030.0, // [00]
            045.0, // [01]
            060.0, // [02] [DEFAULT]
            090.0, // [03]
            120.0, // [04]
            144.0, // [05]
            165.0, // [06]
            240.0, // [07]
            360.0, // [08]
        ];
    }
}
