namespace PixelDust.Game.Constants.Elements
{
    public static class PElementRenderingConstants
    {
        // SPRITES
        public const int SPRITE_DIVISIONS_LENGTH = 4;
        public const int SPRITE_SLICE_SIZE = 16;

        public const float SPRITE_X_OFFSET = 0.5f;
        public const float SPRITE_Y_OFFSET = 0.5f;

        // BLOB
        public const int BLOB_ROTATION_VALUE = 4;

        // (Sprite 1 - Northwest Pivot)
        public const byte BLOB_NORTHWEST_PIVOT_EMPTY = 000;
        public const byte BLOB_NORTHWEST_PIVOT_CASE_1 = 064;
        public const byte BLOB_NORTHWEST_PIVOT_CASE_2 = 128;
        public const byte BLOB_NORTHWEST_PIVOT_CASE_3 = 001;
        public const byte BLOB_NORTHWEST_PIVOT_CASE_4 = 192;
        public const byte BLOB_NORTHWEST_PIVOT_CASE_5 = 003;
        public const byte BLOB_NORTHWEST_PIVOT_CASE_6 = 066;
        public const byte BLOB_NORTHWEST_PIVOT_SURROUNDED = 193;

        // (Sprite 2 - Northeast Pivot)
        public const byte BLOB_NORTHEAST_PIVOT_EMPTY = 000;
        public const byte BLOB_NORTHEAST_PIVOT_CASE_1 = 004;
        public const byte BLOB_NORTHEAST_PIVOT_CASE_2 = 002;
        public const byte BLOB_NORTHEAST_PIVOT_CASE_3 = 001;
        public const byte BLOB_NORTHEAST_PIVOT_CASE_4 = 006;
        public const byte BLOB_NORTHEAST_PIVOT_CASE_5 = 003;
        public const byte BLOB_NORTHEAST_PIVOT_CASE_6 = 005;
        public const byte BLOB_NORTHEAST_PIVOT_SURROUNDED = 007;

        // (Sprite 3 - Southeast Pivot)
        public const byte BLOB_SOUTHEAST_PIVOT_EMPTY = 000;
        public const byte BLOB_SOUTHEAST_PIVOT_CASE_1 = 064;
        public const byte BLOB_SOUTHEAST_PIVOT_CASE_2 = 032;
        public const byte BLOB_SOUTHEAST_PIVOT_CASE_3 = 016;
        public const byte BLOB_SOUTHEAST_PIVOT_CASE_4 = 096;
        public const byte BLOB_SOUTHEAST_PIVOT_CASE_5 = 048;
        public const byte BLOB_SOUTHEAST_PIVOT_CASE_6 = 080;
        public const byte BLOB_SOUTHEAST_PIVOT_SURROUNDED = 112;

        // (Sprite 4 - Southwest Pivot)
        public const byte BLOB_SOUTHWEST_PIVOT_EMPTY = 000;
        public const byte BLOB_SOUTHWEST_PIVOT_CASE_1 = 004;
        public const byte BLOB_SOUTHWEST_PIVOT_CASE_2 = 008;
        public const byte BLOB_SOUTHWEST_PIVOT_CASE_3 = 016;
        public const byte BLOB_SOUTHWEST_PIVOT_CASE_4 = 012;
        public const byte BLOB_SOUTHWEST_PIVOT_CASE_5 = 024;
        public const byte BLOB_SOUTHWEST_PIVOT_CASE_6 = 020;
        public const byte BLOB_SOUTHWEST_PIVOT_SURROUNDED = 028;
    }
}
