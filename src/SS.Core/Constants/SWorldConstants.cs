using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.Core.Constants
{
    public static class SWorldConstants
    {
        public const byte GRID_SCALE = 32;
        public const byte CHUNK_SCALE = 6;
        public const byte CHUNK_DEFAULT_COOLDOWN = 3;
        public const float BACKGROUND_COLOR_DARKENING_FACTOR = 0.5f;

        public static SSize2 WORLD_THUMBNAIL_SIZE => new(23);

        public static readonly SSize2[] WORLD_SIZES_TEMPLATE =
        [
            // (0) 40x23 (920 elements) - Fits entirely within the player's camera
            new SSize2(40, 23), // Small

            // (1) 80x46 (3,680 elements)
            new SSize2(80, 46), // Medium-Small

            // (2) 120x69 (8,280 elements)
            new SSize2(120, 69), // Medium

            // (3) 160x92 (14,720 elements)
            new SSize2(160, 92), // Medium-Large

            // (4) 240x138 (33,120 elements)
            new SSize2(240, 138), // Large

            // (5) 320x184 (58,880 elements)
            new SSize2(320, 184), // Very Large
        ];
    }
}
