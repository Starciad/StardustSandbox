using Microsoft.Xna.Framework;

namespace StardustSandbox.Constants
{
    internal static class ScreenConstants
    {
        internal const float FRAMERATE = 60.0f;

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

        internal static float[] FRAMERATES =>
        [
            030.0f, // [00]
            045.0f, // [01]
            060.0f, // [02] [DEFAULT]
            090.0f, // [03]
            120.0f, // [04]
            144.0f, // [05]
            165.0f, // [06]
            240.0f, // [07]
            360.0f, // [08]
        ];
    }
}
