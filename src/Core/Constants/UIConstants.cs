/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

namespace StardustSandbox.Core.Constants
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

        // Buttons
        internal const byte ELEMENT_BUTTONS_LENGTH = 13;

        // Item Explorer
        internal const byte ITEM_EXPLORER_ITEMS_PER_ROW = 12;
        internal const byte ITEM_EXPLORER_ITEMS_PER_COLUMN = 4;
        internal const byte ITEM_EXPLORER_ITEMS_PER_PAGE = ITEM_EXPLORER_ITEMS_PER_ROW * ITEM_EXPLORER_ITEMS_PER_COLUMN;
        internal const byte ITEM_EXPLORER_SUBCATEGORY_BUTTONS_LENGTH = 14;

        // World Explorer
        internal const byte WORLD_EXPLORER_ITEMS_PER_ROW = 3;
        internal const byte WORLD_EXPLORER_ITEMS_PER_COLUMN = 3;
        internal const byte WORLD_EXPLORER_ITEMS_PER_PAGE = WORLD_EXPLORER_ITEMS_PER_ROW * WORLD_EXPLORER_ITEMS_PER_COLUMN;

        // Achievements
        internal const byte ACHIEVEMENTS_PER_ROW = 15;
        internal const byte ACHIEVEMENTS_PER_COLUMN = 7;
        internal const byte ACHIEVEMENTS_PER_PAGE = ACHIEVEMENTS_PER_ROW * ACHIEVEMENTS_PER_COLUMN;

        // Credits
        internal const float CREDITS_SPEED = 0.05f;
        internal const float CREDITS_VERTICAL_SPACING = 64.0f;

        // Main Menu
        internal const float MAIN_ANIMATION_SPEED = 2.0f;
        internal const float MAIN_ANIMATION_AMPLITUDE = 10.0f;
        internal const float MAIN_BUTTON_ANIMATION_SPEED = 1.5f;
        internal const float MAIN_BUTTON_ANIMATION_AMPLITUDE = 5.0f;

        // Options
        internal const float OPTIONS_SCROLL_STEP = 52.0f;
        internal const float OPTIONS_ITEM_SPACING = 58.0f;

        // Notification Box
        internal const float NOTIFICATION_DISPLAY_DURATION_SECONDS = 5.0f;
        internal const float NOTIFICATION_HIDE_DURATION_SECONDS = 0.5f;
        internal const float NOTIFICATION_MARGIN_LERP_FACTOR = 0.2f;

        #endregion
    }
}
