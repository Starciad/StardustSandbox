namespace StardustSandbox.Game.Constants.Elements
{
    public static class SElementRenderingConstants
    {
        #region SPRITES
        public const byte SPRITE_DIVISIONS_LENGTH = 4;
        public const byte SPRITE_SLICE_SIZE = 16;

        public const float SPRITE_X_OFFSET = 0.5f;
        public const float SPRITE_Y_OFFSET = 0.5f;
        #endregion

        #region BLOB SYSTEM

        /*
         * WARNING!
         * 
         * Be very careful with the constants below. The constant values below
         * were calculated precisely so the system could slice the sprites
         * as planned. Any changes may result in system failure or malfunction.
         * 
         * The sprites that use the blob renderer must comply with the game's 
         * size and texturing standards, otherwise, they will not be compatible 
         * with the project and might be misinterpreted by the system.
         * 
         * Thank you in advance!
         * 
         */

        public const int BLOB_ROTATION_VALUE = 4;

        // (Sprite 1 - Northwest Pivot)
        public const byte BLOB_NORTHWEST_PIVOT_EMPTY = 0;
        public const byte BLOB_NORTHWEST_PIVOT_CASE_1 = 64;
        public const byte BLOB_NORTHWEST_PIVOT_CASE_2 = 128;
        public const byte BLOB_NORTHWEST_PIVOT_CASE_3 = 1;
        public const byte BLOB_NORTHWEST_PIVOT_CASE_4 = 192;
        public const byte BLOB_NORTHWEST_PIVOT_CASE_5 = 129;
        public const byte BLOB_NORTHWEST_PIVOT_CASE_6 = 65;
        public const byte BLOB_NORTHWEST_PIVOT_SURROUNDED = 193;

        // (Sprite 2 - Northeast Pivot)
        public const byte BLOB_NORTHEAST_PIVOT_EMPTY = 0;
        public const byte BLOB_NORTHEAST_PIVOT_CASE_1 = 4;
        public const byte BLOB_NORTHEAST_PIVOT_CASE_2 = 2;
        public const byte BLOB_NORTHEAST_PIVOT_CASE_3 = 1;
        public const byte BLOB_NORTHEAST_PIVOT_CASE_4 = 6;
        public const byte BLOB_NORTHEAST_PIVOT_CASE_5 = 3;
        public const byte BLOB_NORTHEAST_PIVOT_CASE_6 = 5;
        public const byte BLOB_NORTHEAST_PIVOT_SURROUNDED = 7;

        // (Sprite 3 - Southwest Pivot)
        public const byte BLOB_SOUTHWEST_PIVOT_EMPTY = 0;
        public const byte BLOB_SOUTHWEST_PIVOT_CASE_1 = 64;
        public const byte BLOB_SOUTHWEST_PIVOT_CASE_2 = 32;
        public const byte BLOB_SOUTHWEST_PIVOT_CASE_3 = 16;
        public const byte BLOB_SOUTHWEST_PIVOT_CASE_4 = 96;
        public const byte BLOB_SOUTHWEST_PIVOT_CASE_5 = 48;
        public const byte BLOB_SOUTHWEST_PIVOT_CASE_6 = 80;
        public const byte BLOB_SOUTHWEST_PIVOT_SURROUNDED = 112;

        // (Sprite 4 - Southeast Pivot)
        public const byte BLOB_SOUTHEAST_PIVOT_EMPTY = 0;
        public const byte BLOB_SOUTHEAST_PIVOT_CASE_1 = 4;
        public const byte BLOB_SOUTHEAST_PIVOT_CASE_2 = 8;
        public const byte BLOB_SOUTHEAST_PIVOT_CASE_3 = 16;
        public const byte BLOB_SOUTHEAST_PIVOT_CASE_4 = 12;
        public const byte BLOB_SOUTHEAST_PIVOT_CASE_5 = 24;
        public const byte BLOB_SOUTHEAST_PIVOT_CASE_6 = 20;
        public const byte BLOB_SOUTHEAST_PIVOT_SURROUNDED = 28;
        #endregion
    }
}
