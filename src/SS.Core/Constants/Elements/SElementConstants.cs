using Microsoft.Xna.Framework;

using StardustSandbox.Core.Colors;

namespace StardustSandbox.Core.Constants.Elements
{
    public static class SElementConstants
    {
        // Identifiers
        public const string PREFIX_IDENTIFIER = "element_";

        public const string DIRT_IDENTIFIER = PREFIX_IDENTIFIER + "dirt";
        public const string MUD_IDENTIFIER = PREFIX_IDENTIFIER + "mud";
        public const string WATER_IDENTIFIER = PREFIX_IDENTIFIER + "water";
        public const string STONE_IDENTIFIER = PREFIX_IDENTIFIER + "stone";
        public const string GRASS_IDENTIFIER = PREFIX_IDENTIFIER + "grass";
        public const string ICE_IDENTIFIER = PREFIX_IDENTIFIER + "ice";
        public const string SAND_IDENTIFIER = PREFIX_IDENTIFIER + "sand";
        public const string SNOW_IDENTIFIER = PREFIX_IDENTIFIER + "snow";
        public const string MOVABLE_CORRUPTION_IDENTIFIER = PREFIX_IDENTIFIER + "movable_corruption";
        public const string LAVA_IDENTIFIER = PREFIX_IDENTIFIER + "lava";
        public const string ACID_IDENTIFIER = PREFIX_IDENTIFIER + "acid";
        public const string GLASS_IDENTIFIER = PREFIX_IDENTIFIER + "glass";
        public const string IRON_IDENTIFIER = PREFIX_IDENTIFIER + "iron";
        public const string WALL_IDENTIFIER = PREFIX_IDENTIFIER + "wall";
        public const string WOOD_IDENTIFIER = PREFIX_IDENTIFIER + "wood";
        public const string GAS_CORRUPTION_IDENTIFIER = PREFIX_IDENTIFIER + "gas_corruption";
        public const string LIQUID_CORRUPTION_IDENTIFIER = PREFIX_IDENTIFIER + "liquid_corruption";
        public const string IMMOVABLE_CORRUPTION_IDENTIFIER = PREFIX_IDENTIFIER + "immovable_corruption";
        public const string STEAM_IDENTIFIER = PREFIX_IDENTIFIER + "steam";
        public const string SMOKE_IDENTIFIER = PREFIX_IDENTIFIER + "smoke";
        public const string RED_BRICK_IDENTIFIER = PREFIX_IDENTIFIER + "red_brick";
        public const string TREE_LEAF_IDENTIFIER = PREFIX_IDENTIFIER + "tree_leaf";
        public const string MOUNTING_BLOCK_IDENTIFIER = PREFIX_IDENTIFIER + "mounting_block";
        public const string FIRE_IDENTIFIER = PREFIX_IDENTIFIER + "fire";
        public const string LAMP_IDENTIFIER = PREFIX_IDENTIFIER + "lamp";
        public const string VOID_IDENTIFIER = PREFIX_IDENTIFIER + "void";
        public const string CLONE_IDENTIFIER = PREFIX_IDENTIFIER + "clone";
        public const string OIL_IDENTIFIER = PREFIX_IDENTIFIER + "oil";
        public const string SALT_IDENTIFIER = PREFIX_IDENTIFIER + "salt";
        public const string SALTWATER_IDENTIFIER = PREFIX_IDENTIFIER + "saltwater";
        public const string BOMB_IDENTIFIER = PREFIX_IDENTIFIER + "bomb";
        public const string DYNAMITE_IDENTIFIER = PREFIX_IDENTIFIER + "dynamite";
        public const string TNT_IDENTIFIER = PREFIX_IDENTIFIER + "tnt";
        public const string DRY_SPONGE_IDENTIFIER = PREFIX_IDENTIFIER + "dry_sponge";
        public const string WET_SPONGE_IDENTIFIER = PREFIX_IDENTIFIER + "wet_sponge";
        public const string GOLD_IDENTIFIER = PREFIX_IDENTIFIER + "gold";

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
