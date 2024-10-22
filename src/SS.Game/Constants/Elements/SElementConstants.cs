using Microsoft.Xna.Framework;

namespace StardustSandbox.Game.Constants.Elements
{
    public static class SElementConstants
    {
        // Corruption
        public const int CHANCE_OF_CORRUPTION_TO_SPREAD_TOTAL = 100;
        public const int CHANCE_OF_CORRUPTION_TO_SPREAD = 4;

        // Mounting Block
        public static readonly Color[] COLORS_OF_MOUNTING_BLOCKS = [
            new(032, 120, 248), // Blue
            new(080, 208, 080), // Green
            new(248, 096, 080), // Red
            new(248, 197, 080), // Yellow
            new(248, 080, 125), // Pink
        ];
    }
}
