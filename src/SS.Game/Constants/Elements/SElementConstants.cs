using Microsoft.Xna.Framework;

namespace StardustSandbox.Game.Constants.Elements
{
    public static class SElementConstants
    {
        // Corruption
        public const int CHANCE_OF_CORRUPTION_TO_SPREAD_TOTAL = 100;
        public const int CHANCE_OF_CORRUPTION_TO_SPREAD = 4;

        // Fire
        public const int CHANCE_OF_FIRE_TO_DISAPPEAR_TOTAL = 100;
        public const int CHANCE_OF_FIRE_TO_DISAPPEAR = 15;

        public const int CHANCE_FOR_FIRE_TO_LEAVE_SMOKE_TOTAL = 100;
        public const int CHANCE_FOR_FIRE_TO_LEAVE_SMOKE = 25;

        public const int FIRE_HEAT_VALUE = 5;
        public const int CHANCE_OF_COMBUSTION = 25;

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
