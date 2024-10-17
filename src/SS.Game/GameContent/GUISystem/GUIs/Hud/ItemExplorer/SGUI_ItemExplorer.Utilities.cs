using StardustSandbox.Game.Constants.GUI.Common;
using StardustSandbox.Game.Items;

using System;

namespace StardustSandbox.Game.GameContent.GUISystem.GUIs.Hud.ItemExplorer
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

            this.selectedItems = GetSelectedItems(category.Items, startIndex, endIndex);

            UpdateItemCatalog();
        }

        private static SItem[] GetSelectedItems(SItem[] items, int startIndex, int endIndex)
        {
            int length = endIndex - startIndex;

            SItem[] selectedItems = new SItem[length];

            for (int i = 0; i < length; i++)
            {
                selectedItems[i] = items[startIndex + i];
            }

            return selectedItems;
        }
    }
}
