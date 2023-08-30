using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelDust.Core.Engine
{
    /// <summary>
    /// Static class that contains information related to the screen/window of the game.
    /// </summary>
    public static class PScreen
    {
        /// <summary>
        /// Standard resolution used as a reference by the project.
        /// </summary>
        /// <remarks>
        /// As a starting point for rendering operations and other settings, a value of 1280x720 is used.
        /// </remarks>
        public static Vector2 DefaultResolution => resolutions[3];

        /// <summary>
        /// Resolution the game is currently at.
        /// </summary>
        public static Vector2 CurrentResolution { get; private set; }

        internal static Viewport Viewport => _viewport;

        private static Viewport _viewport;

        // 16:9 Aspect Ratio
        private static readonly Vector2[] resolutions = new Vector2[]
        {
            new(640, 360), // nHD
            new(854, 480), // FWVGA
            new(960, 540), // qHD
            new(1280, 720), // SD / HD ready (720p)
            new(1366, 768), // WXGA
            new(1600, 900), // HD+
            new(1920, 1080), // FHD / Full HD (1080p)
            new(2560, 1440), // WQHD
            new(3200, 1800), // QHD+
            new(3840, 2160), // 4K UHD
            new(5120, 2880), // 5K
            new(7680, 4320), // 8K UHD
            new(15360, 8640) // 16K UHD
        };

        internal static void Build(Viewport viewport)
        {
            _viewport = viewport;
        }
    }
}
