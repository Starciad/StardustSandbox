using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Informational;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Global;
using StardustSandbox.Core.Constants.GUI;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Items;
using StardustSandbox.Core.Mathematics.Primitives;

using System;
using System.Linq;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    public sealed partial class SGUI_ItemExplorer(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUI_HUD guiHUD, SGUITooltipBoxElement tooltipBoxElementElement) : SGUISystem(gameInstance, identifier, guiEvents)
    {
        private string selectedCategoryName;
        private int selectedPageIndex;
        private SItem[] selectedItems;

        private readonly Texture2D particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
        private readonly Texture2D guiBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_1");
        private readonly Texture2D squareShapeTexture = gameInstance.AssetDatabase.GetTexture("shape_square_1");

        private readonly SGUI_HUD _guiHUD = guiHUD;
        private readonly SGUITooltipBoxElement tooltipBoxElement = tooltipBoxElementElement;

        protected override void OnLoad()
        {
            this.SGameInstance.GameManager.GameState.IsSimulationPaused = true;
            SelectItemCatalog("powders", 0);
        }

        protected override void OnUnload()
        {
            this.SGameInstance.GameManager.GameState.IsSimulationPaused = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.tooltipBoxElement.IsVisible = false;

            UpdateCategoryButtons();
            UpdateItemCatalog();

            this.tooltipBoxElement.RefreshDisplay(SGUIGlobalTooltip.Title, SGUIGlobalTooltip.Description);
        }

        private void UpdateCategoryButtons()
        {
            for (int i = 0; i < this.categoryButtonSlots.Length; i++)
            {
                (SGUIImageElement categoryButtonBackground, _) = this.categoryButtonSlots[i];

                // Check if the mouse clicked on the current slot.
                if (this.GUIEvents.OnMouseClick(categoryButtonBackground.Position, new SSize2(SHUDConstants.SLOT_SIZE)))
                {
                    SelectItemCatalog((string)categoryButtonBackground.GetData(SItemExplorerConstants.DATA_FILED_CATEGORY_ID), 0);
                }

                bool isOver = this.GUIEvents.OnMouseOver(categoryButtonBackground.Position, new SSize2(SHUDConstants.SLOT_SIZE));

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    if (!this.tooltipBoxElement.HasContent)
                    {
                        SItemCategory category = this.SGameInstance.ItemDatabase.GetCategoryById((string)categoryButtonBackground.GetData(SItemExplorerConstants.DATA_FILED_CATEGORY_ID));

                        SGUIGlobalTooltip.Title = category.DisplayName;
                        SGUIGlobalTooltip.Description = category.Description;
                    }
                }
            }
        }

        private void UpdateItemCatalog()
        {
            for (int i = 0; i < this.itemButtonSlots.Length; i++)
            {
                (SGUIImageElement itemSlotBackground, _) = this.itemButtonSlots[i];

                if (!itemSlotBackground.IsVisible)
                {
                    continue;
                }

                if (this.GUIEvents.OnMouseClick(itemSlotBackground.Position, new SSize2(SHUDConstants.SLOT_SIZE)))
                {
                    this._guiHUD.AddItemToToolbar((string)itemSlotBackground.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID));

                    this.SGameInstance.GUIManager.CloseGUI(SGUIConstants.HUD_ELEMENT_EXPLORER_IDENTIFIER);
                    this.SGameInstance.GUIManager.ShowGUI(SGUIConstants.HUD_IDENTIFIER);
                }

                bool isOver = this.GUIEvents.OnMouseOver(itemSlotBackground.Position, new SSize2(SHUDConstants.SLOT_SIZE));

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    if (!this.tooltipBoxElement.HasContent)
                    {
                        SItem item = this.SGameInstance.ItemDatabase.GetItemById((string)itemSlotBackground.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID));

                        SGUIGlobalTooltip.Title = item.DisplayName;
                        SGUIGlobalTooltip.Description = item.Description;
                    }
                }

                itemSlotBackground.Color = isOver ? Color.DarkGray : Color.White;
            }
        }


        // ============================================== //

        private void SelectItemCatalog(string categoryId, int pageIndex)
        {
            SelectItemCatalog(this.SGameInstance.ItemDatabase.GetCategoryById(categoryId), pageIndex);
        }

        private void SelectItemCatalog(SItemCategory category, int pageIndex)
        {
            this.explorerTitleLabel.SetTextualContent(category.DisplayName);

            this.selectedCategoryName = category.Identifier;
            this.selectedPageIndex = pageIndex;

            int itemsPerPage = SItemExplorerConstants.ITEMS_PER_PAGE;

            int startIndex = pageIndex * itemsPerPage;
            int endIndex = startIndex + itemsPerPage;

            endIndex = Math.Min(endIndex, this.SGameInstance.ItemDatabase.TotalItemCount);

            this.selectedItems = [.. category.Items.Take(new Range(startIndex, endIndex - startIndex))];

            ChangeItemCatalog();
        }

        private void ChangeItemCatalog()
        {
            for (int i = 0; i < this.itemButtonSlots.Length; i++)
            {
                (SGUIImageElement itemSlotBackground, SGUIImageElement itemSlotIcon) = this.itemButtonSlots[i];

                if (i < this.selectedItems.Length)
                {
                    itemSlotBackground.IsVisible = true;
                    itemSlotIcon.IsVisible = true;

                    SItem item = this.selectedItems[i];
                    itemSlotIcon.Texture = item.IconTexture;

                    // Add or Update Data
                    if (!itemSlotBackground.ContainsData(SHUDConstants.DATA_FILED_ELEMENT_ID))
                    {
                        itemSlotBackground.AddData(SHUDConstants.DATA_FILED_ELEMENT_ID, item.Identifier);
                    }
                    else
                    {
                        itemSlotBackground.UpdateData(SHUDConstants.DATA_FILED_ELEMENT_ID, item.Identifier);
                    }
                }
                else
                {
                    itemSlotBackground.IsVisible = false;
                    itemSlotIcon.IsVisible = false;
                }
            }
        }
    }
}
