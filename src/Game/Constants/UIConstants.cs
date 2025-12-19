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

        #region GUIs

        #region HUD

        // Buttons
        internal const byte ELEMENT_BUTTONS_LENGTH = 13;

        // Item Explorer
        internal const byte HUD_ITEM_EXPLORER_ITEMS_PER_ROW = 12;
        internal const byte HUD_ITEM_EXPLORER_ITEMS_PER_COLUMN = 4;
        internal const byte HUD_ITEM_EXPLORER_ITEMS_PER_PAGE = HUD_ITEM_EXPLORER_ITEMS_PER_ROW * HUD_ITEM_EXPLORER_ITEMS_PER_COLUMN;

        internal const byte HUD_ITEM_EXPLORER_SUB_CATEGORY_BUTTONS_LENGTH = 14;

        // World Explorer
        internal const byte HUD_WORLD_EXPLORER_ITEMS_PER_ROW = 3;
        internal const byte HUD_WORLD_EXPLORER_ITEMS_PER_COLUMN = 3;
        internal const byte HUD_WORLD_EXPLORER_ITEMS_PER_PAGE = HUD_WORLD_EXPLORER_ITEMS_PER_ROW * HUD_WORLD_EXPLORER_ITEMS_PER_COLUMN;

        #endregion

        #endregion
    }
}
