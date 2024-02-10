using PixelDust.Game.Mathematics;

namespace PixelDust.Game.Constants
{
    public static class PScreenConstants
    {
        public const int SCREEN_WIDTH = 0;
        public const int SCREEN_HEIGHT = 0;

        // 16:9 Aspect Ratio
        public static Size2Int[] RESOLUTIONS =>
        [
            new Size2Int(640, 360), // nHD
            new Size2Int(854, 480), // FWVGA
            new Size2Int(960, 540), // qHD
            new Size2Int(1280, 720), // SD / HD ready (720p)
            new Size2Int(1366, 768), // WXGA
            new Size2Int(1600, 900), // HD+
            new Size2Int(1920, 1080), // FHD / Full HD (1080p)
            new Size2Int(2560, 1440), // WQHD
            new Size2Int(3200, 1800), // QHD+
            new Size2Int(3840, 2160), // 4K UHD
            new Size2Int(5120, 2880), // 5K
            new Size2Int(7680, 4320), // 8K UHD
            new Size2Int(15360, 8640) // 16K UHD
        ];
    }
}
