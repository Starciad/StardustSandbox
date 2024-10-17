using StardustSandbox.Game.Constants.GUI.Common;
using StardustSandbox.Game.Items;

using System;
using System.Linq;

namespace StardustSandbox.Game.GameContent.GUI.Content.Menus.ItemExplorer
{
    public sealed partial class SGUI_ItemExplorer
    {
        private void SelectItemCatalog(int categoryIndex, int pageIndex)
        {
            SelectItemCatalog(this.SGameInstance.ItemDatabase.Categories[categoryIndex], pageIndex);
        }

        private void SelectItemCatalog(SItemCategory category, int pageIndex)
        {
            this.explorerTitleLabel.SetTextContent(category.DisplayName);

            this.selectedCategoryName = category.Identifier;
            this.selectedPageIndex = pageIndex;

            int itemsPerPage = SItemExplorerConstants.ITEMS_PER_PAGE;

            int startIndex = pageIndex * itemsPerPage;
            int endIndex = startIndex + itemsPerPage;

            endIndex = Math.Min(endIndex, this.SGameInstance.ItemDatabase.Items.Length);

            this.selectedItems = [.. category.Items.Take(new Range(startIndex, endIndex - startIndex))];

            UpdateItemCatalog();
        }
    }
}
