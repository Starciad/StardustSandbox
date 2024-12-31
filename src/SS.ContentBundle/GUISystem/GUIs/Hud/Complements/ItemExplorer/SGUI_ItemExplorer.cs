using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Informational;
using StardustSandbox.ContentBundle.GUISystem.Global;
using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.ContentBundle.Localization.GUIs;
using StardustSandbox.Core.Catalog;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.GUI;
using StardustSandbox.Core.Constants.GUI.Common;
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
        private int selectedPageIndex;

        private IEnumerable<SItem> selectedItems;
        private int selectedItemsLength;

        private readonly Texture2D particleTexture;
        private readonly Texture2D guiBackgroundTexture;
        private readonly Texture2D guiButton1Texture;
        private readonly Texture2D[] iconTextures;
        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly SGUI_HUD guiHUD;
        private readonly SGUITooltipBoxElement tooltipBoxElement;

        private readonly SButton[] menuButtons;

        public SGUI_ItemExplorer(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUI_HUD guiHUD, SGUITooltipBoxElement tooltipBoxElementElement) : base(gameInstance, identifier, guiEvents)
        {
            this.selectedCategory = gameInstance.CatalogDatabase.GetCategory("elements");
            this.selectedSubcategory = this.selectedCategory.GetSubcategory("powders");
            this.selectedPageIndex = 0;

            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_1");
            this.guiButton1Texture = gameInstance.AssetDatabase.GetTexture("gui_button_1");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_2");

            this.iconTextures = [
                gameInstance.AssetDatabase.GetTexture("icon_gui_16"),
            ];

            this.guiHUD = guiHUD;
            this.tooltipBoxElement = tooltipBoxElementElement;

            this.menuButtons = [
                new(this.iconTextures[0], SLocalization_GUIs.Button_Exit_Name, SLocalization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.menuButtonSlots = new SSlot[this.menuButtons.Length];
            this.itemButtonSlots = new SSlot[SItemExplorerConstants.ITEMS_PER_PAGE];
            this.categoryButtonSlots = new SSlot[this.SGameInstance.CatalogDatabase.TotalCategoryCount];
            this.subcategoryButtonSlots = new SSlot[SItemExplorerConstants.SUB_CATEGORY_BUTTONS_LENGTH];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.tooltipBoxElement.IsVisible = false;

            UpdateMenuButtons();
            UpdateCategoryButtons();
            UpdateSubcategoryButtons();
            UpdateItemCatalog();

            this.tooltipBoxElement.RefreshDisplay(SGUIGlobalTooltip.Title, SGUIGlobalTooltip.Description);
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                SSlot slot = this.menuButtonSlots[i];

                Vector2 position = slot.BackgroundElement.Position;
                SSize2 size = new(SHUDConstants.SLOT_SIZE);

                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    this.menuButtons[i].ClickAction.Invoke();
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

                // Check if the mouse clicked on the current slot.
                if (this.GUIEvents.OnMouseClick(categorySlot.BackgroundElement.Position, new(SItemExplorerConstants.SLOT_SIZE)))
                {
                    SelectItemCatalog(category, category.Subcategories.First(), 0);
                }

                bool isOver = this.GUIEvents.OnMouseOver(categorySlot.BackgroundElement.Position, new(SItemExplorerConstants.SLOT_SIZE));

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

                // Check if the mouse clicked on the current slot.
                if (this.GUIEvents.OnMouseClick(subcategorySlot.BackgroundElement.Position, new(SItemExplorerConstants.SLOT_SIZE)))
                {
                    SelectItemCatalog(subcategory.Parent, subcategory, 0);
                }

                bool isOver = this.GUIEvents.OnMouseOver(subcategorySlot.BackgroundElement.Position, new(SItemExplorerConstants.SLOT_SIZE));

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

                if (this.GUIEvents.OnMouseClick(slot.BackgroundElement.Position, new(SItemExplorerConstants.SLOT_SIZE)))
                {
                    this.guiHUD.AddItemToToolbar(item);
                    this.SGameInstance.GUIManager.CloseGUI();
                }

                bool isOver = this.GUIEvents.OnMouseOver(slot.BackgroundElement.Position, new(SItemExplorerConstants.SLOT_SIZE));

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
