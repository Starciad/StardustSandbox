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
            // (0) Small
            new SSize2(40, 23),

            // (1) Medium
            new SSize2(80, 46),

            // (2) Large
            new SSize2(120, 69),

            // (3)
            new SSize2(160, 92),

            // (4)
            new SSize2(200, 155),

            // (5)
            new SSize2(240, 178),
        ];
    }
}
