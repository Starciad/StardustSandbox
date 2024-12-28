using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Informational;
using StardustSandbox.ContentBundle.GUISystem.Global;
using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.Core.Catalog;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.GUI;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_ItemExplorer : SGUISystem
    {
        private SCategory selectedCategory;
        private SSubcategory selectedSubcategory;
        private int selectedPageIndex;

        private IEnumerable<SItem> selectedItems;
        private int selectedItemsLength;

        private readonly Texture2D particleTexture;
        private readonly Texture2D guiBackgroundTexture;
        private readonly Texture2D guiButton1Texture;
        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly SGUI_HUD guiHUD;
        private readonly SGUITooltipBoxElement tooltipBoxElement;

        public SGUI_ItemExplorer(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUI_HUD guiHUD, SGUITooltipBoxElement tooltipBoxElementElement) : base(gameInstance, identifier, guiEvents)
        {
            this.selectedCategory = gameInstance.CatalogDatabase.GetCategory("elements");
            this.selectedSubcategory = selectedCategory.GetSubcategory("powders");
            this.selectedPageIndex = 0;

            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_1");
            this.guiButton1Texture = gameInstance.AssetDatabase.GetTexture("gui_button_1");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_2");

            this.guiHUD = guiHUD;
            this.tooltipBoxElement = tooltipBoxElementElement;

            this.itemButtonSlots = new SSlot[SItemExplorerConstants.ITEMS_PER_PAGE];
            this.categoryButtonSlots = new SSlot[this.SGameInstance.CatalogDatabase.TotalCategoryCount];
            this.subcategoryButtonSlots = new SSlot[SItemExplorerConstants.SUB_CATEGORY_BUTTONS_LENGTH];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.tooltipBoxElement.IsVisible = false;

            UpdateCategoryButtons();
            UpdateSubcategoryButtons();
            UpdateItemCatalog();

            this.tooltipBoxElement.RefreshDisplay(SGUIGlobalTooltip.Title, SGUIGlobalTooltip.Description);
        }

        private void UpdateCategoryButtons()
        {
            for (int i = 0; i < this.categoryButtonSlots.Length; i++)
            {
                SSlot categorySlot = this.categoryButtonSlots[i];
                SCategory category;

                // Check if the mouse clicked on the current slot.
                if (this.GUIEvents.OnMouseClick(categorySlot.BackgroundElement.Position, new(SItemExplorerConstants.SLOT_SIZE)))
                {
                    category = (SCategory)categorySlot.BackgroundElement.GetData(SGUIConstants.DATA_CATEGORY);

                    SelectItemCatalog(category, category.Subcategories.First(), 0);
                }

                bool isOver = this.GUIEvents.OnMouseOver(categorySlot.BackgroundElement.Position, new(SItemExplorerConstants.SLOT_SIZE));

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    if (!this.tooltipBoxElement.HasContent)
                    {
                        category = (SCategory)categorySlot.BackgroundElement.GetData(SGUIConstants.DATA_CATEGORY);

                        SGUIGlobalTooltip.Title = category.DisplayName;
                        SGUIGlobalTooltip.Description = category.Description;
                    }
                }
            }
        }

        private void UpdateSubcategoryButtons()
        {
            for (int i = 0; i < this.subcategoryButtonSlots.Length; i++)
            {
                SSlot subcategorySlot = this.subcategoryButtonSlots[i];
                SSubcategory subcategory;

                // Check if the mouse clicked on the current slot.
                if (this.GUIEvents.OnMouseClick(subcategorySlot.BackgroundElement.Position, new(SItemExplorerConstants.SLOT_SIZE)))
                {
                    subcategory = (SSubcategory)subcategorySlot.BackgroundElement.GetData(SGUIConstants.DATA_SUBCATEGORY);

                    SelectItemCatalog(subcategory.Parent, subcategory, 0);
                }

                bool isOver = this.GUIEvents.OnMouseOver(subcategorySlot.BackgroundElement.Position, new(SItemExplorerConstants.SLOT_SIZE));

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    if (!this.tooltipBoxElement.HasContent)
                    {
                        subcategory = (SSubcategory)subcategorySlot.BackgroundElement.GetData(SGUIConstants.DATA_SUBCATEGORY);

                        SGUIGlobalTooltip.Title = subcategory.DisplayName;
                        SGUIGlobalTooltip.Description = subcategory.Description;
                    }
                }
            }
        }

        private void UpdateItemCatalog()
        {
            for (int i = 0; i < this.itemButtonSlots.Length; i++)
            {
                SSlot slot = this.itemButtonSlots[i];
                SItem item;

                if (!slot.BackgroundElement.IsVisible)
                {
                    continue;
                }

                if (this.GUIEvents.OnMouseClick(slot.BackgroundElement.Position, new(SItemExplorerConstants.SLOT_SIZE)))
                {
                    item = (SItem)slot.BackgroundElement.GetData(SGUIConstants.DATA_ITEM);

                    this.guiHUD.AddItemToToolbar(item);
                    this.SGameInstance.GUIManager.CloseGUI();
                }

                bool isOver = this.GUIEvents.OnMouseOver(slot.BackgroundElement.Position, new(SItemExplorerConstants.SLOT_SIZE));

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    if (!this.tooltipBoxElement.HasContent)
                    {
                        item = (SItem)slot.BackgroundElement.GetData(SGUIConstants.DATA_ITEM);

                        SGUIGlobalTooltip.Title = item.DisplayName;
                        SGUIGlobalTooltip.Description = item.Description;
                    }
                }

                slot.BackgroundElement.Color = isOver ? SColorPalette.LemonYellow : SColorPalette.White;
            }
        }

        // ============================================== //

        private void SelectItemCatalog(SCategory category, SSubcategory subcategory, int pageIndex)
        {
            this.explorerTitleLabel.SetTextualContent(subcategory.DisplayName);

            this.selectedCategory = category;
            this.selectedSubcategory = subcategory;
            this.selectedPageIndex = pageIndex;

            int startIndex = pageIndex * SItemExplorerConstants.ITEMS_PER_PAGE;
            int endIndex = startIndex + SItemExplorerConstants.ITEMS_PER_PAGE;

            endIndex = Math.Min(endIndex, this.SGameInstance.CatalogDatabase.TotalItemCount);

            this.selectedItems = subcategory.Items.Take(new Range(startIndex, endIndex - startIndex));
            this.selectedItemsLength = this.selectedItems.Count();

            ChangeSubcategorCatalog();
            ChangeItemCatalog();
        }

        private void ChangeSubcategorCatalog()
        {
            for (int i = 0; i < this.subcategoryButtonSlots.Length; i++)
            {
                SSlot subcategorySlot = this.subcategoryButtonSlots[i];

                if (i < this.selectedCategory.SubcategoriesCount)
                {
                    subcategorySlot.Show();

                    SSubcategory subcategory = this.selectedCategory.Subcategories.ElementAt(i);
                    subcategorySlot.IconElement.Texture = subcategory.IconTexture;

                    // Add or Update Data
                    if (!subcategorySlot.BackgroundElement.ContainsData(SGUIConstants.DATA_SUBCATEGORY))
                    {
                        subcategorySlot.BackgroundElement.AddData(SGUIConstants.DATA_SUBCATEGORY, subcategory);
                    }
                    else
                    {
                        subcategorySlot.BackgroundElement.UpdateData(SGUIConstants.DATA_SUBCATEGORY, subcategory);
                    }
                }
                else
                {
                    subcategorySlot.Hide();
                }
            }
        }

        private void ChangeItemCatalog()
        {
            for (int i = 0; i < this.itemButtonSlots.Length; i++)
            {
                SSlot itemSlot = this.itemButtonSlots[i];

                if (i < this.selectedItemsLength)
                {
                    itemSlot.Show();

                    SItem item = this.selectedItems.ElementAt(i);
                    itemSlot.IconElement.Texture = item.IconTexture;

                    // Add or Update Data
                    if (!itemSlot.BackgroundElement.ContainsData(SGUIConstants.DATA_ITEM))
                    {
                        itemSlot.BackgroundElement.AddData(SGUIConstants.DATA_ITEM, item);
                    }
                    else
                    {
                        itemSlot.BackgroundElement.UpdateData(SGUIConstants.DATA_ITEM, item);
                    }
                }
                else
                {
                    itemSlot.Hide();
                }
            }
        }
    }
}
