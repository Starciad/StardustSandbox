using Microsoft.Xna.Framework;

using StardustSandbox.Core.Colors;

namespace StardustSandbox.Core.Constants.Elements
{
    public static class SElementConstants
    {
        // Identifiers
        public const string IDENTIFIER_PREFIX = "element_";

        public const string IDENTIFIER_DIRT = IDENTIFIER_PREFIX + "dirt";
        public const string IDENTIFIER_MUD = IDENTIFIER_PREFIX + "mud";
        public const string IDENTIFIER_WATER = IDENTIFIER_PREFIX + "water";
        public const string IDENTIFIER_STONE = IDENTIFIER_PREFIX + "stone";
        public const string IDENTIFIER_GRASS = IDENTIFIER_PREFIX + "grass";
        public const string IDENTIFIER_ICE = IDENTIFIER_PREFIX + "ice";
        public const string IDENTIFIER_SAND = IDENTIFIER_PREFIX + "sand";
        public const string IDENTIFIER_SNOW = IDENTIFIER_PREFIX + "snow";
        public const string IDENTIFIER_MOVABLE_CORRUPTION = IDENTIFIER_PREFIX + "movable_corruption";
        public const string IDENTIFIER_LAVA = IDENTIFIER_PREFIX + "lava";
        public const string IDENTIFIER_ACID = IDENTIFIER_PREFIX + "acid";
        public const string IDENTIFIER_GLASS = IDENTIFIER_PREFIX + "glass";
        public const string IDENTIFIER_IRON = IDENTIFIER_PREFIX + "iron";
        public const string IDENTIFIER_WALL = IDENTIFIER_PREFIX + "wall";
        public const string IDENTIFIER_WOOD = IDENTIFIER_PREFIX + "wood";
        public const string IDENTIFIER_GAS_CORRUPTION = IDENTIFIER_PREFIX + "gas_corruption";
        public const string IDENTIFIER_LIQUID_CORRUPTION = IDENTIFIER_PREFIX + "liquid_corruption";
        public const string IDENTIFIER_IMMOVABLE_CORRUPTION = IDENTIFIER_PREFIX + "immovable_corruption";
        public const string IDENTIFIER_STEAM = IDENTIFIER_PREFIX + "steam";
        public const string IDENTIFIER_SMOKE = IDENTIFIER_PREFIX + "smoke";
        public const string IDENTIFIER_RED_BRICK = IDENTIFIER_PREFIX + "red_brick";
        public const string IDENTIFIER_TREE_LEAF = IDENTIFIER_PREFIX + "tree_leaf";
        public const string IDENTIFIER_MOUNTING_BLOCK = IDENTIFIER_PREFIX + "mounting_block";
        public const string IDENTIFIER_FIRE = IDENTIFIER_PREFIX + "fire";
        public const string IDENTIFIER_LAMP = IDENTIFIER_PREFIX + "lamp";
        public const string IDENTIFIER_VOID = IDENTIFIER_PREFIX + "void";
        public const string IDENTIFIER_CLONE = IDENTIFIER_PREFIX + "clone";

        // Corruption
        public const byte CHANCE_OF_CORRUPTION_TO_SPREAD_TOTAL = 100;
        public const byte CHANCE_OF_CORRUPTION_TO_SPREAD = 4;

        // Fire
        public const byte CHANCE_OF_FIRE_TO_DISAPPEAR_TOTAL = 100;
        public const byte CHANCE_OF_FIRE_TO_DISAPPEAR = 20;

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
