using StardustSandbox.Game.Mathematics.Primitives;

namespace StardustSandbox.Game.Constants
{
    public static class SWorldConstants
    {
        public const byte GRID_SCALE = 32;
        public const byte CHUNK_SCALE = 6;

        public static readonly SSize2[] WORLD_SIZES_TEMPLATE =
        [
            // (0) Small
            new SSize2(40, 23),

            // (1) Medium
            new SSize2(80, 46),

            // (2) Large
            new SSize2(160, 92),
        ];
    }
}
