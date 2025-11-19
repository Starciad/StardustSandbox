namespace StardustSandbox.Constants
{
    internal static class UIConstants
    {
        #region Data

        internal const string DATA_ITEM = "item";
        internal const string DATA_CATEGORY = "category";
        internal const string DATA_SUBCATEGORY = "subcategory";
        internal const string DATA_LANGUAGE_CODE = "language_code";

        #endregion

        #region Elements

        internal const byte SPRITE_SLICE_SIZE = 32;

        #endregion

        #region GUIs

        #region HUD

        // Slots
        internal const byte HUD_SLOT_SCALE = 2;
        internal const byte HUD_GRID_SIZE = 32;
        internal const byte HUD_SLOT_SPACING = HUD_GRID_SIZE * HUD_SLOT_SCALE;

        // Buttons
        internal const byte HUD_ELEMENT_BUTTONS_LENGTH = 13;

        // Information
        internal const byte HUD_INFORMATION_AMOUNT = 5;

        // Item Explorer
        internal const byte HUD_ITEM_EXPLORER_ITEMS_PER_ROW = 12;
        internal const byte HUD_ITEM_EXPLORER_ITEMS_PER_COLUMN = 4;
        internal const byte HUD_ITEM_EXPLORER_ITEMS_PER_PAGE = HUD_ITEM_EXPLORER_ITEMS_PER_ROW * HUD_ITEM_EXPLORER_ITEMS_PER_COLUMN;

        internal const byte HUD_ITEM_EXPLORER_SLOT_SCALE = 2;
        internal const byte HUD_ITEM_EXPLORER_GRID_SIZE = 32;
        internal const byte HUD_ITEM_EXPLORER_SLOT_SPACING = HUD_ITEM_EXPLORER_GRID_SIZE * HUD_ITEM_EXPLORER_SLOT_SCALE;

        internal const byte HUD_ITEM_EXPLORER_SUB_CATEGORY_BUTTONS_LENGTH = 14;

        // World Explorer
        internal const byte HUD_WORLD_EXPLORER_ITEMS_PER_ROW = 3;
        internal const byte HUD_WORLD_EXPLORER_ITEMS_PER_COLUMN = 3;
        internal const byte HUD_WORLD_EXPLORER_ITEMS_PER_PAGE = HUD_WORLD_EXPLORER_ITEMS_PER_ROW * HUD_WORLD_EXPLORER_ITEMS_PER_COLUMN;

        internal const int HUD_WORLD_EXPLORER_SLOT_WIDTH = 386;
        internal const int HUD_WORLD_EXPLORER_SLOT_HEIGHT = 140;

        internal const int HUD_WORLD_EXPLORER_SLOT_WIDTH_SPACING = HUD_WORLD_EXPLORER_SLOT_WIDTH + 32;
        internal const int HUD_WORLD_EXPLORER_SLOT_HEIGHT_SPACING = HUD_WORLD_EXPLORER_SLOT_HEIGHT + 32;

        #endregion

        #endregion
    }
}
