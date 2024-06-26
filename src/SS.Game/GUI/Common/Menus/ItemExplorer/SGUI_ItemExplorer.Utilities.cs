﻿using StardustSandbox.Game.Constants.GUI.Common;

using System;
using System.Linq;

namespace StardustSandbox.Game.GUI.Common.Menus.ItemExplorer
{
    public sealed partial class SGUI_ItemExplorer
    {
        private void SelectItemCatalog(int categoryIndex, int pageIndex)
        {
            SelectItemCatalog(this.Game.ItemDatabase.Categories[categoryIndex], pageIndex);
        }

        private void SelectItemCatalog(string categoryName, int pageIndex)
        {
            this.explorerTitleLabel.SetTextContent(categoryName);

            this.selectedCategoryName = categoryName;
            this.selectedPageIndex = pageIndex;

            int itemsPerPage = SItemExplorerConstants.ITEMS_PER_PAGE;

            int startIndex = pageIndex * itemsPerPage;
            int endIndex = startIndex + itemsPerPage;

            endIndex = Math.Min(endIndex, this.Game.ItemDatabase.Items.Length);

            this.selectedItems = [.. this.Game.ItemDatabase.GetCatalogItems(categoryName).Take(new Range(startIndex, endIndex - startIndex))];

            UpdateItemCatalog();
        }
    }
}
