using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Informational;
using StardustSandbox.ContentBundle.GUISystem.Global;
using StardustSandbox.ContentBundle.GUISystem.Helpers.General;
using StardustSandbox.ContentBundle.GUISystem.Helpers.Interactive;
using StardustSandbox.ContentBundle.Localization.GUIs;
using StardustSandbox.ContentBundle.Localization.Statements;
using StardustSandbox.Core.Catalog;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.GUISystem;
using StardustSandbox.Core.Constants.GUISystem.GUIs.Hud;
using StardustSandbox.Core.Constants.GUISystem.GUIs.Hud.Complements;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics.Primitives;

using System;
using System.Collections.Generic;
using System.Linq;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_ItemExplorer : SGUISystem
    {
        private SCategory selectedCategory;
        private SSubcategory selectedSubcategory;

        private int currentPage;
        private int totalPages;

        private IEnumerable<SItem> selectedItems;
        private int selectedItemsLength;

        private readonly Texture2D particleTexture;
        private readonly Texture2D panelBackgroundTexture;
        private readonly Texture2D guiButton1Texture;
        private readonly Texture2D exitIconTexture;
        private readonly Texture2D rightIconTexture;
        private readonly Texture2D leftIconTexture;
        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly SGUI_HUD guiHUD;
        private readonly SGUITooltipBoxElement tooltipBoxElement;

        private readonly SButton[] menuButtons;
        private readonly SButton[] paginationButtons;

        public SGUI_ItemExplorer(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUI_HUD guiHUD, SGUITooltipBoxElement tooltipBoxElementElement) : base(gameInstance, identifier, guiEvents)
        {
            this.selectedCategory = gameInstance.CatalogDatabase.GetCategory("elements");
            this.selectedSubcategory = this.selectedCategory.GetSubcategory("powders");

            this.currentPage = 0;
            this.totalPages = 0;

            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.panelBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_12");
            this.guiButton1Texture = gameInstance.AssetDatabase.GetTexture("gui_button_1");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_2");

            this.exitIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_16");
            this.rightIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_48");
            this.leftIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_50");

            this.guiHUD = guiHUD;
            this.tooltipBoxElement = tooltipBoxElementElement;

            this.menuButtons = [
                new(this.exitIconTexture, SLocalization_Statements.Exit, SLocalization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.paginationButtons = [
                new(this.leftIconTexture, "Left", string.Empty, PreviousButtonAction),
                new(this.rightIconTexture, "Right", string.Empty, NextButtonAction),
            ];

            this.menuButtonSlots = new SSlot[this.menuButtons.Length];
            this.itemButtonSlots = new SSlot[SGUI_ItemExplorerConstants.ITEMS_PER_PAGE];
            this.categoryButtonSlots = new SSlot[this.SGameInstance.CatalogDatabase.TotalCategoryCount];
            this.subcategoryButtonSlots = new SSlot[SGUI_ItemExplorerConstants.SUB_CATEGORY_BUTTONS_LENGTH];
            this.paginationButtonSlots = new SSlot[this.paginationButtons.Length];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.tooltipBoxElement.IsVisible = false;

            UpdateMenuButtons();
            UpdateCategoryButtons();
            UpdateSubcategoryButtons();
            UpdateItemCatalog();
            UpdatePagination();

            this.tooltipBoxElement.RefreshDisplay(SGUIGlobalTooltip.Title, SGUIGlobalTooltip.Description);
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                SSlot slot = this.menuButtonSlots[i];

                Vector2 position = slot.BackgroundElement.Position;
                SSize2 size = new(SGUI_HUDConstants.SLOT_SIZE);

                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                if (this.GUIEvents.OnMouseOver(position, size))
                {
                    this.tooltipBoxElement.IsVisible = true;

                    SGUIGlobalTooltip.Title = this.menuButtons[i].DisplayName;
                    SGUIGlobalTooltip.Description = this.menuButtons[i].Description;

                    slot.BackgroundElement.Color = SColorPalette.HoverColor;
                }
                else
                {
                    slot.BackgroundElement.Color = SColorPalette.White;
                }
            }
        }

        private void UpdateCategoryButtons()
        {
            for (int i = 0; i < this.categoryButtonSlots.Length; i++)
            {
                SSlot categorySlot = this.categoryButtonSlots[i];

                if (!categorySlot.BackgroundElement.IsVisible)
                {
                    continue;
                }

                SCategory category = (SCategory)categorySlot.BackgroundElement.GetData(SGUIConstants.DATA_CATEGORY);

                Vector2 position = categorySlot.BackgroundElement.Position;
                SSize2 size = new(SGUI_ItemExplorerConstants.SLOT_SIZE);

                // Check if the mouse clicked on the current slot.
                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    SelectItemCatalog(category, category.Subcategories.First(), 0);
                }

                bool isOver = this.GUIEvents.OnMouseOver(position, size);

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    SGUIGlobalTooltip.Title = category.DisplayName;
                    SGUIGlobalTooltip.Description = category.Description;
                }
            }
        }

        private void UpdateSubcategoryButtons()
        {
            for (int i = 0; i < this.subcategoryButtonSlots.Length; i++)
            {
                SSlot subcategorySlot = this.subcategoryButtonSlots[i];

                if (!subcategorySlot.BackgroundElement.IsVisible)
                {
                    continue;
                }

                SSubcategory subcategory = (SSubcategory)subcategorySlot.BackgroundElement.GetData(SGUIConstants.DATA_SUBCATEGORY);

                Vector2 position = subcategorySlot.BackgroundElement.Position;
                SSize2 size = new(SGUI_ItemExplorerConstants.SLOT_SIZE);

                // Check if the mouse clicked on the current slot.
                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    SelectItemCatalog(subcategory.Parent, subcategory, 0);
                }

                bool isOver = this.GUIEvents.OnMouseOver(position, size);

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    SGUIGlobalTooltip.Title = subcategory.DisplayName;
                    SGUIGlobalTooltip.Description = subcategory.Description;
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

                SItem item = (SItem)slot.BackgroundElement.GetData(SGUIConstants.DATA_ITEM);

                Vector2 position = slot.BackgroundElement.Position;
                SSize2 size = new(SGUI_ItemExplorerConstants.SLOT_SIZE);

                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    this.guiHUD.AddItemToToolbar(item);
                    this.SGameInstance.GUIManager.CloseGUI();
                }

                bool isOver = this.GUIEvents.OnMouseOver(position, size);

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    SGUIGlobalTooltip.Title = item.DisplayName;
                    SGUIGlobalTooltip.Description = item.Description;

                    slot.BackgroundElement.Color = SColorPalette.HoverColor;
                }
                else
                {
                    slot.BackgroundElement.Color = SColorPalette.White;
                }

                if (this.guiHUD.ItemIsEquipped(item))
                {
                    slot.BackgroundElement.Color = SColorPalette.TealGray;
                }
            }
        }

        private void UpdatePagination()
        {
            for (int i = 0; i < this.paginationButtons.Length; i++)
            {
                SSlot slot = this.paginationButtonSlots[i];

                Vector2 position = slot.BackgroundElement.Position;
                SSize2 size = new(SGUI_ItemExplorerConstants.SLOT_SIZE);

                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    this.paginationButtons[i].ClickAction?.Invoke();
                }

                slot.BackgroundElement.Color = this.GUIEvents.OnMouseOver(position, size) ? SColorPalette.HoverColor : SColorPalette.White;
            }
        }

        // ============================================== //

        private void SelectItemCatalog(SCategory category, SSubcategory subcategory, int pageIndex)
        {
            this.menuTitleElement.SetTextualContent(subcategory.DisplayName);

            this.selectedCategory = category;
            this.selectedSubcategory = subcategory;
            this.currentPage = pageIndex;
            this.totalPages = subcategory.Items.Count() / SGUI_ItemExplorerConstants.ITEMS_PER_PAGE;

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
            this.pageIndexLabelElement.SetTextualContent(string.Concat(this.currentPage + 1, " / ", this.totalPages + 1));

            int startIndex = this.currentPage * SGUI_ItemExplorerConstants.ITEMS_PER_PAGE;
            int endIndex = Math.Min(startIndex + SGUI_ItemExplorerConstants.ITEMS_PER_PAGE, this.SGameInstance.CatalogDatabase.TotalItemCount);

            this.selectedItems = this.selectedSubcategory.Items.Take(new Range(startIndex, endIndex));
            this.selectedItemsLength = this.selectedItems.Count();

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
