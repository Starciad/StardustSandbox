using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Catalog;
using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UISystem;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.UISystem.Elements;
using StardustSandbox.UISystem.Elements.Graphics;
using StardustSandbox.UISystem.Elements.Textual;
using StardustSandbox.UISystem.Utilities;

using System;

namespace StardustSandbox.UISystem.UIs.HUD
{
    internal sealed class ItemExplorerUI : UI
    {
        private Category selectedCategory;
        private Subcategory selectedSubcategory;

        private int currentPage;
        private int totalPages;
        private int selectedItemsLength;

        private Item[] selectedItems;

        private ImageUIElement panelBackgroundElement;

        private LabelUIElement menuTitleElement;
        private LabelUIElement pageIndexLabelElement;

        private readonly TooltipBoxUIElement tooltipBoxElement;

        private readonly UIButton[] menuButtons;
        private readonly UIButton[] paginationButtons;

        private readonly UISlot[] menuButtonSlots;
        private readonly UISlot[] itemButtonSlots;
        private readonly UISlot[] categoryButtonSlots;
        private readonly UISlot[] subcategoryButtonSlots;
        private readonly UISlot[] paginationButtonSlots;

        private readonly Texture2D particleTexture;
        private readonly Texture2D panelBackgroundTexture;
        private readonly Texture2D guiSmallButtonTexture;
        private readonly Texture2D exitIconTexture;
        private readonly Texture2D rightIconTexture;
        private readonly Texture2D leftIconTexture;
        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly HudUI hudUI;

        private readonly GameManager gameManager;
        private readonly UIManager uiManager;

        internal ItemExplorerUI(
            GameManager gameManager,
            UIIndex index,
            HudUI hudUI,
            TooltipBoxUIElement tooltipBoxElement,
            UIManager uiManager
        ) : base(index)
        {
            this.gameManager = gameManager;
            this.uiManager = uiManager;

            this.selectedCategory = CatalogDatabase.GetCategory(0);
            this.selectedSubcategory = this.selectedCategory.GetSubcategory(0);

            this.currentPage = 0;
            this.totalPages = 0;

            this.particleTexture = AssetDatabase.GetTexture("texture_particle_1");
            this.panelBackgroundTexture = AssetDatabase.GetTexture("texture_gui_background_12");
            this.guiSmallButtonTexture = AssetDatabase.GetTexture("texture_gui_button_1");
            this.bigApple3PMSpriteFont = AssetDatabase.GetSpriteFont("font_2");

            this.exitIconTexture = AssetDatabase.GetTexture("texture_icon_gui_16");
            this.rightIconTexture = AssetDatabase.GetTexture("texture_icon_gui_48");
            this.leftIconTexture = AssetDatabase.GetTexture("texture_icon_gui_50");

            this.hudUI = hudUI;
            this.tooltipBoxElement = tooltipBoxElement;

            this.menuButtons = [
                new(this.exitIconTexture, Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.paginationButtons = [
                new(this.leftIconTexture, "Left", string.Empty, PreviousButtonAction),
                new(this.rightIconTexture, "Right", string.Empty, NextButtonAction),
            ];

            this.menuButtonSlots = new UISlot[this.menuButtons.Length];
            this.itemButtonSlots = new UISlot[UIConstants.HUD_ITEM_EXPLORER_ITEMS_PER_PAGE];
            this.categoryButtonSlots = new UISlot[CatalogDatabase.CategoryLength];
            this.subcategoryButtonSlots = new UISlot[UIConstants.HUD_ITEM_EXPLORER_SUB_CATEGORY_BUTTONS_LENGTH];
            this.paginationButtonSlots = new UISlot[this.paginationButtons.Length];
        }

        #region ACTION

        // Menu
        private void ExitButtonAction()
        {
            this.uiManager.CloseGUI();
        }

        // Pagination
        private void PreviousButtonAction()
        {
            if (this.currentPage > 0)
            {
                this.currentPage--;
            }
            else
            {
                this.currentPage = this.totalPages;
            }

            ChangeItemCatalog();
        }

        private void NextButtonAction()
        {
            if (this.currentPage < this.totalPages)
            {
                this.currentPage++;
            }
            else
            {
                this.currentPage = 0;
            }

            ChangeItemCatalog();
        }

        #endregion

        #region BUILDER

        protected override void OnBuild(Layout layout)
        {
            BuildBackground(layout);
            BuildTitle(layout);
            BuildMenuButtons(layout);
            BuildItemCatalogSlots(layout);
            BuildCategoryButtons(layout);
            BuildSubcategoryButtons(layout);
            BuildPagination(layout);

            layout.AddElement(this.tooltipBoxElement);
        }

        private void BuildBackground(Layout layout)
        {
            ImageUIElement backgroundShadowElement = new()
            {
                Texture = this.particleTexture,
                Scale = new(ScreenConstants.DEFAULT_SCREEN_WIDTH, ScreenConstants.DEFAULT_SCREEN_HEIGHT),
                Size = new(1),
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            this.panelBackgroundElement = new()
            {
                Texture = this.panelBackgroundTexture,
                Size = new(1084, 607),
                Margin = new(98, 90),
            };

            this.panelBackgroundElement.PositionRelativeToScreen();

            layout.AddElement(backgroundShadowElement);
            layout.AddElement(this.panelBackgroundElement);
        }

        private void BuildTitle(Layout layout)
        {
            this.menuTitleElement = new()
            {
                SpriteFont = this.bigApple3PMSpriteFont,
                Scale = new(0.12f),
                PositionAnchor = CardinalDirection.Northwest,
                OriginPivot = CardinalDirection.East,
                Margin = new(32, 40),
                Color = AAP64ColorPalette.White,
            };

            this.menuTitleElement.SetTextualContent(Localization_GUIs.HUD_Complements_ItemExplorer_Title);
            this.menuTitleElement.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(3f));
            this.menuTitleElement.PositionRelativeToElement(this.panelBackgroundElement);

            layout.AddElement(this.menuTitleElement);
        }

        private void BuildMenuButtons(Layout layout)
        {
            Vector2 margin = new(-32f, -40f);

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                UIButton button = this.menuButtons[i];

                ImageUIElement backgroundElement = new()
                {
                    Texture = this.guiSmallButtonTexture,
                    Scale = new(UIConstants.HUD_SLOT_SCALE),
                    Size = new(UIConstants.HUD_GRID_SIZE),
                    Margin = margin,
                };

                ImageUIElement iconElement = new()
                {
                    Texture = button.IconTexture,
                    OriginPivot = CardinalDirection.Center,
                    Scale = new(1.5f),
                    Size = new(UIConstants.HUD_GRID_SIZE)
                };

                UISlot slot = new(backgroundElement, iconElement);

                slot.BackgroundElement.PositionAnchor = CardinalDirection.Northeast;
                slot.BackgroundElement.OriginPivot = CardinalDirection.Center;

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.panelBackgroundElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.menuButtonSlots[i] = slot;

                // Spacing
                margin.X -= UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);

                layout.AddElement(backgroundElement);
                layout.AddElement(iconElement);
            }
        }

        private void BuildItemCatalogSlots(Layout layout)
        {
            Vector2 slotMargin = new(96, 192);

            int index = 0;
            for (int col = 0; col < UIConstants.HUD_ITEM_EXPLORER_ITEMS_PER_COLUMN; col++)
            {
                for (int row = 0; row < UIConstants.HUD_ITEM_EXPLORER_ITEMS_PER_ROW; row++)
                {
                    ImageUIElement slotBackground = new()
                    {
                        Texture = this.guiSmallButtonTexture,
                        OriginPivot = CardinalDirection.Center,
                        Scale = new(UIConstants.HUD_ITEM_EXPLORER_SLOT_SCALE),
                        PositionAnchor = CardinalDirection.Northwest,
                        Size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE),
                        Margin = slotMargin
                    };

                    ImageUIElement slotIcon = new()
                    {
                        OriginPivot = CardinalDirection.Center,
                        Scale = new(1.5f),
                        Size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE)
                    };

                    // Position
                    slotBackground.PositionRelativeToElement(this.panelBackgroundElement);
                    slotIcon.PositionRelativeToElement(slotBackground);

                    // Spacing
                    slotMargin.X += UIConstants.HUD_ITEM_EXPLORER_SLOT_SPACING + (UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE / 2);
                    this.itemButtonSlots[index] = new(slotBackground, slotIcon);
                    index++;

                    // Adding
                    layout.AddElement(slotBackground);
                    layout.AddElement(slotIcon);
                }

                slotMargin.X = 96;
                slotMargin.Y += UIConstants.HUD_ITEM_EXPLORER_SLOT_SPACING + (UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE / 2);
            }
        }

        private void BuildCategoryButtons(Layout layout)
        {
            Vector2 slotMargin = new(32f, -40f);

            int index = 0;

            for (int i = 0; i < CatalogDatabase.Categories.Length; i++)
            {
                Category category = CatalogDatabase.Categories[i];

                ImageUIElement slotBackground = new()
                {
                    Texture = this.guiSmallButtonTexture,
                    OriginPivot = CardinalDirection.Center,
                    PositionAnchor = CardinalDirection.Northwest,
                    Scale = new(UIConstants.HUD_ITEM_EXPLORER_SLOT_SCALE),
                    Size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE),
                    Margin = slotMargin
                };

                ImageUIElement slotIcon = new()
                {
                    Texture = category.IconTexture,
                    OriginPivot = CardinalDirection.Center,
                    Scale = new(1.5f),
                    Size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE)
                };

                // Data
                if (!slotBackground.ContainsData(UIConstants.DATA_CATEGORY))
                {
                    slotBackground.AddData(UIConstants.DATA_CATEGORY, category);
                }

                // Position
                slotBackground.PositionRelativeToElement(this.panelBackgroundElement);
                slotIcon.PositionRelativeToElement(slotBackground);

                // Spacing
                slotMargin.X += UIConstants.HUD_ITEM_EXPLORER_SLOT_SPACING + (UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE / 2);
                this.categoryButtonSlots[index] = new(slotBackground, slotIcon);
                index++;

                // Adding
                layout.AddElement(slotBackground);
                layout.AddElement(slotIcon);
            }
        }

        private void BuildSubcategoryButtons(Layout layout)
        {
            int index = 0;
            int sideCounts = UIConstants.HUD_ITEM_EXPLORER_SUB_CATEGORY_BUTTONS_LENGTH / 2;

            Vector2 margin = new(-48, 48);
            BuildSlots(CardinalDirection.Northwest);
            margin = new(48, 48);
            BuildSlots(CardinalDirection.Northeast);

            // =============================== //

            void BuildSlots(CardinalDirection positionAnchor)
            {
                for (int i = 0; i < sideCounts; i++)
                {
                    ImageUIElement slotBackground = new()
                    {
                        Texture = this.guiSmallButtonTexture,
                        PositionAnchor = positionAnchor,
                        OriginPivot = CardinalDirection.Center,
                        Scale = new(UIConstants.HUD_ITEM_EXPLORER_SLOT_SCALE),
                        Size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE),
                        Margin = margin
                    };

                    ImageUIElement slotIcon = new()
                    {
                        OriginPivot = CardinalDirection.Center,
                        Scale = new(1.5f),
                        Size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE)
                    };

                    // Position
                    slotBackground.PositionRelativeToElement(this.panelBackgroundElement);
                    slotIcon.PositionRelativeToElement(slotBackground);

                    // Spacing
                    margin.Y += UIConstants.HUD_ITEM_EXPLORER_SLOT_SPACING + (UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE / 2);
                    this.subcategoryButtonSlots[index] = new(slotBackground, slotIcon);
                    index++;

                    // Adding
                    layout.AddElement(slotBackground);
                    layout.AddElement(slotIcon);
                }
            }
        }

        private void BuildPagination(Layout layout)
        {
            this.pageIndexLabelElement = new()
            {
                Scale = new(0.1f),
                SpriteFont = this.bigApple3PMSpriteFont,
                PositionAnchor = CardinalDirection.South,
                OriginPivot = CardinalDirection.Center,
                Margin = new(0f, -35f),
            };

            this.pageIndexLabelElement.SetTextualContent("1 / 1");
            this.pageIndexLabelElement.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(2f));
            this.pageIndexLabelElement.PositionRelativeToElement(this.panelBackgroundElement);

            layout.AddElement(this.pageIndexLabelElement);

            for (int i = 0; i < this.paginationButtons.Length; i++)
            {
                ImageUIElement slotBackground = new()
                {
                    Texture = this.guiSmallButtonTexture,
                    OriginPivot = CardinalDirection.Center,
                    Scale = new(1.5f),
                    Size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE),
                };

                ImageUIElement slotIcon = new()
                {
                    Texture = this.paginationButtons[i].IconTexture,
                    OriginPivot = CardinalDirection.Center,
                    Scale = new(1f),
                    Size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE)
                };

                // Spacing
                this.paginationButtonSlots[i] = new(slotBackground, slotIcon);

                // Adding
                layout.AddElement(slotBackground);
                layout.AddElement(slotIcon);
            }

            // Left
            UISlot leftSlot = this.paginationButtonSlots[0];
            leftSlot.BackgroundElement.PositionAnchor = CardinalDirection.Southwest;
            leftSlot.BackgroundElement.Margin = new(34f, -34f);

            // Right
            UISlot rightSlot = this.paginationButtonSlots[1];
            rightSlot.BackgroundElement.PositionAnchor = CardinalDirection.Southeast;
            rightSlot.BackgroundElement.Margin = new(-34f);

            foreach (UISlot slot in this.paginationButtonSlots)
            {
                slot.BackgroundElement.PositionRelativeToElement(this.panelBackgroundElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);
            }
        }

        #endregion

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.tooltipBoxElement.IsVisible = false;

            UpdateMenuButtons();
            UpdateCategoryButtons();
            UpdateSubcategoryButtons();
            UpdateItemCatalog();
            UpdatePagination();

            this.tooltipBoxElement.RefreshDisplay(TooltipContent.Title, TooltipContent.Description);
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                UISlot slot = this.menuButtonSlots[i];

                Vector2 position = slot.BackgroundElement.Position;
                Vector2 size = new(UIConstants.HUD_GRID_SIZE);

                if (UIInteraction.OnMouseClick(position, size))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                if (UIInteraction.OnMouseOver(position, size))
                {
                    this.tooltipBoxElement.IsVisible = true;

                    TooltipContent.Title = this.menuButtons[i].Name;
                    TooltipContent.Description = this.menuButtons[i].Description;

                    slot.BackgroundElement.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.BackgroundElement.Color = AAP64ColorPalette.White;
                }
            }
        }

        private void UpdateCategoryButtons()
        {
            for (int i = 0; i < this.categoryButtonSlots.Length; i++)
            {
                UISlot categorySlot = this.categoryButtonSlots[i];

                if (!categorySlot.BackgroundElement.IsVisible)
                {
                    continue;
                }

                Category category = (Category)categorySlot.BackgroundElement.GetData(UIConstants.DATA_CATEGORY);

                Vector2 position = categorySlot.BackgroundElement.Position;
                Vector2 size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE);

                // Check if the mouse clicked on the current slot.
                if (UIInteraction.OnMouseClick(position, size))
                {
                    SelectItemCatalog(category, category.Subcategories[0], 0);
                }

                bool isOver = UIInteraction.OnMouseOver(position, size);

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    TooltipContent.Title = category.Name;
                    TooltipContent.Description = category.Description;
                }
            }
        }

        private void UpdateSubcategoryButtons()
        {
            for (int i = 0; i < this.subcategoryButtonSlots.Length; i++)
            {
                UISlot subcategorySlot = this.subcategoryButtonSlots[i];

                if (!subcategorySlot.BackgroundElement.IsVisible)
                {
                    continue;
                }

                Subcategory subcategory = (Subcategory)subcategorySlot.BackgroundElement.GetData(UIConstants.DATA_SUBCATEGORY);

                Vector2 position = subcategorySlot.BackgroundElement.Position;
                Vector2 size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE);

                // Check if the mouse clicked on the current slot.
                if (UIInteraction.OnMouseClick(position, size))
                {
                    SelectItemCatalog(subcategory.ParentCategory, subcategory, 0);
                }

                bool isOver = UIInteraction.OnMouseOver(position, size);

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    TooltipContent.Title = subcategory.Name;
                    TooltipContent.Description = subcategory.Description;
                }
            }
        }

        private void UpdateItemCatalog()
        {
            for (int i = 0; i < this.itemButtonSlots.Length; i++)
            {
                UISlot slot = this.itemButtonSlots[i];

                if (!slot.BackgroundElement.IsVisible)
                {
                    continue;
                }

                Item item = (Item)slot.BackgroundElement.GetData(UIConstants.DATA_ITEM);

                Vector2 position = slot.BackgroundElement.Position;
                Vector2 size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE);

                if (UIInteraction.OnMouseClick(position, size))
                {
                    this.hudUI.AddItemToToolbar(item);
                    this.uiManager.CloseGUI();
                }

                bool isOver = UIInteraction.OnMouseOver(position, size);

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    TooltipContent.Title = item.Name;
                    TooltipContent.Description = item.Description;

                    slot.BackgroundElement.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.BackgroundElement.Color = AAP64ColorPalette.White;
                }

                if (this.hudUI.ItemIsEquipped(item))
                {
                    slot.BackgroundElement.Color = AAP64ColorPalette.TealGray;
                }
            }
        }

        private void UpdatePagination()
        {
            for (int i = 0; i < this.paginationButtons.Length; i++)
            {
                UISlot slot = this.paginationButtonSlots[i];

                Vector2 position = slot.BackgroundElement.Position;
                Vector2 size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE);

                if (UIInteraction.OnMouseClick(position, size))
                {
                    this.paginationButtons[i].ClickAction?.Invoke();
                }

                slot.BackgroundElement.Color = UIInteraction.OnMouseOver(position, size) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        #endregion

        #region UTILITIES

        private void SelectItemCatalog(Category category, Subcategory subcategory, int pageIndex)
        {
            this.menuTitleElement.SetTextualContent(subcategory.Name);

            this.selectedCategory = category;
            this.selectedSubcategory = subcategory;
            this.currentPage = pageIndex;
            this.totalPages = subcategory.Items.Length / UIConstants.HUD_ITEM_EXPLORER_ITEMS_PER_PAGE;

            ChangeSubcategorCatalog();
            ChangeItemCatalog();
        }

        private void ChangeSubcategorCatalog()
        {
            for (int i = 0; i < this.subcategoryButtonSlots.Length; i++)
            {
                UISlot subcategorySlot = this.subcategoryButtonSlots[i];

                if (i < this.selectedCategory.SubcategoriesLength)
                {
                    subcategorySlot.Show();

                    Subcategory subcategory = this.selectedCategory.Subcategories[i];
                    subcategorySlot.IconElement.Texture = subcategory.IconTexture;

                    // Add or Update Data
                    if (!subcategorySlot.BackgroundElement.ContainsData(UIConstants.DATA_SUBCATEGORY))
                    {
                        subcategorySlot.BackgroundElement.AddData(UIConstants.DATA_SUBCATEGORY, subcategory);
                    }
                    else
                    {
                        subcategorySlot.BackgroundElement.UpdateData(UIConstants.DATA_SUBCATEGORY, subcategory);
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

            int startIndex = this.currentPage * UIConstants.HUD_ITEM_EXPLORER_ITEMS_PER_PAGE;
            int endIndex = Math.Min(startIndex + UIConstants.HUD_ITEM_EXPLORER_ITEMS_PER_PAGE, this.selectedSubcategory.ItemLength);

            this.selectedItems = this.selectedSubcategory.GetItems(startIndex, endIndex);
            this.selectedItemsLength = this.selectedItems.Length;

            for (int i = 0; i < this.itemButtonSlots.Length; i++)
            {
                UISlot itemSlot = this.itemButtonSlots[i];

                if (i < this.selectedItemsLength)
                {
                    itemSlot.Show();

                    Item item = this.selectedItems[i];

                    if (item == null)
                    {
                        continue;
                    }

                    itemSlot.IconElement.Texture = item.IconTexture;

                    // Add or Update Data
                    if (!itemSlot.BackgroundElement.ContainsData(UIConstants.DATA_ITEM))
                    {
                        itemSlot.BackgroundElement.AddData(UIConstants.DATA_ITEM, item);
                    }
                    else
                    {
                        itemSlot.BackgroundElement.UpdateData(UIConstants.DATA_ITEM, item);
                    }
                }
                else
                {
                    itemSlot.Hide();
                }
            }
        }

        #endregion

        #region EVENTS

        protected override void OnOpened()
        {
            this.gameManager.SetState(GameStates.IsCriticalMenuOpen);
            SelectItemCatalog(this.selectedCategory, this.selectedSubcategory, this.currentPage);
        }

        protected override void OnClosed()
        {
            this.gameManager.RemoveState(GameStates.IsCriticalMenuOpen);
        }

        #endregion
    }
}
