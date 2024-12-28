namespace StardustSandbox.Core.Constants.GUI.Common
{
    public static class SItemExplorerConstants
    {
        public const byte ITEMS_PER_PAGE = ITEMS_PER_ROW * ITEMS_PER_COLUMN;
        public const byte ITEMS_PER_ROW = 12;
        public const byte ITEMS_PER_COLUMN = 4;

        public const byte SUB_CATEGORY_BUTTONS_LENGTH = 14;

        public const byte SLOT_SCALE = 2;
        public const byte SLOT_SIZE = 32;
        public const byte SLOT_SPACING = SLOT_SIZE * SLOT_SCALE;

        // Data Fields
        public const string DATA_ITEM = "item";
        public const string DATA_CATEGORY = "category";
        public const string DATA_SUBCATEGORY = "subcategory";
    }
}
