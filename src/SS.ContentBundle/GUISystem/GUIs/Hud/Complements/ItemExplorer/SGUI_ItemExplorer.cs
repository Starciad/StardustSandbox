using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Informational;
using StardustSandbox.ContentBundle.GUISystem.Global;
using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Fonts;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Items;

using System;
using System.Linq;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_ItemExplorer(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUI_HUD guiHUD, SGUITooltipBoxElement tooltipBoxElementElement) : SGUISystem(gameInstance, identifier, guiEvents)
    {
        private string selectedCategoryName;
        private int selectedPageIndex;

        private SItem[] selectedItems;

        private readonly Texture2D particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
        private readonly Texture2D guiBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_1");
        private readonly Texture2D guiButton1Texture = gameInstance.AssetDatabase.GetTexture("gui_button_1");
        private readonly SpriteFont bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_2");

        private readonly SGUI_HUD _guiHUD = guiHUD;
        private readonly SGUITooltipBoxElement tooltipBoxElement = tooltipBoxElementElement;

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
                SSlot slot = this.categoryButtonSlots[i];

                // Check if the mouse clicked on the current slot.
                if (this.GUIEvents.OnMouseClick(slot.BackgroundElement.Position, new(SHUDConstants.SLOT_SIZE)))
                {
                    SelectItemCatalog((string)slot.BackgroundElement.GetData(SItemExplorerConstants.DATA_FILED_CATEGORY_ID), 0);
                }

                bool isOver = this.GUIEvents.OnMouseOver(slot.BackgroundElement.Position, new(SHUDConstants.SLOT_SIZE));

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    if (!this.tooltipBoxElement.HasContent)
                    {
                        SItemCategory category = this.SGameInstance.ItemDatabase.GetCategoryById((string)slot.BackgroundElement.GetData(SItemExplorerConstants.DATA_FILED_CATEGORY_ID));

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
                SSlot slot = this.itemButtonSlots[i];

                if (!slot.BackgroundElement.IsVisible)
                {
                    continue;
                }

                if (this.GUIEvents.OnMouseClick(slot.BackgroundElement.Position, new(SHUDConstants.SLOT_SIZE)))
                {
                    this._guiHUD.AddItemToToolbar((string)slot.BackgroundElement.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID));

                    this.SGameInstance.GUIManager.CloseGUI();
                }

                bool isOver = this.GUIEvents.OnMouseOver(slot.BackgroundElement.Position, new(SHUDConstants.SLOT_SIZE));

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    if (!this.tooltipBoxElement.HasContent)
                    {
                        SItem item = this.SGameInstance.ItemDatabase.GetItemById((string)slot.BackgroundElement.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID));

                        SGUIGlobalTooltip.Title = item.DisplayName;
                        SGUIGlobalTooltip.Description = item.Description;
                    }
                }

                slot.BackgroundElement.Color = isOver ? SColorPalette.LemonYellow : SColorPalette.White;
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
                SSlot slot = this.itemButtonSlots[i];

                if (i < this.selectedItems.Length)
                {
                    slot.BackgroundElement.IsVisible = true;
                    slot.IconElement.IsVisible = true;

                    SItem item = this.selectedItems[i];
                    slot.IconElement.Texture = item.IconTexture;

                    // Add or Update Data
                    if (!slot.BackgroundElement.ContainsData(SHUDConstants.DATA_FILED_ELEMENT_ID))
                    {
                        slot.BackgroundElement.AddData(SHUDConstants.DATA_FILED_ELEMENT_ID, item.Identifier);
                    }
                    else
                    {
                        slot.BackgroundElement.UpdateData(SHUDConstants.DATA_FILED_ELEMENT_ID, item.Identifier);
                    }
                }
                else
                {
                    slot.BackgroundElement.IsVisible = false;
                    slot.IconElement.IsVisible = false;
                }
            }
        }
    }
}
