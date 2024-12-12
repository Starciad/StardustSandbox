using Microsoft.Xna.Framework;

using StardustSandbox.Core.Colors;

namespace StardustSandbox.Core.Constants.Elements
{
    public static class SElementConstants
    {
        // Corruption
        public const byte CHANCE_OF_CORRUPTION_TO_SPREAD_TOTAL = 100;
        public const byte CHANCE_OF_CORRUPTION_TO_SPREAD = 4;

        // Fire
        public const byte CHANCE_OF_FIRE_TO_DISAPPEAR_TOTAL = 100;
        public const byte CHANCE_OF_FIRE_TO_DISAPPEAR = 15;

        public const byte CHANCE_FOR_FIRE_TO_LEAVE_SMOKE_TOTAL = 100;
        public const byte CHANCE_FOR_FIRE_TO_LEAVE_SMOKE = 25;

        public const byte FIRE_HEAT_VALUE = 5;
        public const byte CHANCE_OF_COMBUSTION = 25;

        // Mounting Block
        public static readonly Color[] COLORS_OF_MOUNTING_BLOCKS = [
            SColorPalette.NavyBlue, // Blue
            SColorPalette.DarkGreen, // Green
            SColorPalette.DarkRed, // Red
            SColorPalette.LemonYellow, // Yellow
            SColorPalette.Violet, // Pink
        ];
    }
}
