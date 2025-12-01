using Microsoft.Xna.Framework;

using StardustSandbox.Catalog;
using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;

using System;

namespace StardustSandbox.UI.Common.HUD
{
    internal sealed class ItemExplorerUI : UIBase
    {
        private Category selectedCategory;
        private Subcategory selectedSubcategory;

        private int currentPage;
        private int totalPages;
        private int selectedItemsLength;

        private Item[] selectedItems;

        private Image panelBackgroundElement;

        private Label menuTitleElement;
        private Label pageIndexLabelElement;

        private readonly TooltipBox tooltipBox;

        private readonly ButtonInfo[] menuButtons;
        private readonly ButtonInfo[] paginationButtons;

        private readonly SlotInfo[] menuButtonSlots;
        private readonly SlotInfo[] itemButtonSlots;
        private readonly SlotInfo[] categoryButtonSlots;
        private readonly SlotInfo[] subcategoryButtonSlots;
        private readonly SlotInfo[] paginationButtonSlots;

        private readonly HudUI hudUI;

        private readonly GameManager gameManager;
        private readonly UIManager uiManager;

        internal ItemExplorerUI(
            GameManager gameManager,
            UIIndex index,
            HudUI hudUI,
            TooltipBox tooltipBox,
            UIManager uiManager
        ) : base(index)
        {
            this.gameManager = gameManager;
            this.uiManager = uiManager;

            this.selectedCategory = CatalogDatabase.GetCategory(0);
            this.selectedSubcategory = this.selectedCategory.GetSubcategory(0);

            this.currentPage = 0;
            this.totalPages = 0;

            this.hudUI = hudUI;
            this.tooltipBox = tooltipBox;

            this.menuButtons = [
                new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.paginationButtons = [
                new(TextureIndex.IconUI, new(128, 160, 32, 32), "Left", string.Empty, PreviousButtonAction),
                new(TextureIndex.IconUI, new(64, 160, 32, 32), "Right", string.Empty, NextButtonAction),
            ];

            this.menuButtonSlots = new SlotInfo[this.menuButtons.Length];
            this.itemButtonSlots = new SlotInfo[UIConstants.HUD_ITEM_EXPLORER_ITEMS_PER_PAGE];
            this.categoryButtonSlots = new SlotInfo[CatalogDatabase.CategoryLength];
            this.subcategoryButtonSlots = new SlotInfo[UIConstants.HUD_ITEM_EXPLORER_SUB_CATEGORY_BUTTONS_LENGTH];
            this.paginationButtonSlots = new SlotInfo[this.paginationButtons.Length];
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

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildTitle();
            BuildMenuButtons();
            BuildItemCatalogSlots();
            BuildCategoryButtons();
            BuildSubcategoryButtons();
            BuildPagination(root);

            root.AddChild(this.tooltipBox);
        }

        private void BuildBackground(Container root)
        {
            Image backgroundShadowElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Size = new(1),
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            this.panelBackgroundElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.UIBackgroundItemExplorer),
                Size = new(1084, 607),
                Margin = new(98, 90),
            };

            root.AddChild(backgroundShadowElement);
            root.AddChild(this.panelBackgroundElement);
        }

        private void BuildTitle()
        {
            this.menuTitleElement = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.12f),
                Alignment = CardinalDirection.Northwest,
                Margin = new(32, 40),
                Color = AAP64ColorPalette.White,
                TextContent = Localization_GUIs.HUD_Complements_ItemExplorer_Title,

                BorderDirections = LabelBorderDirection.All,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderOffset = 3f,
                BorderThickness = 3f,
            };

            this.panelBackgroundElement.AddChild(this.menuTitleElement);
        }

        private void BuildMenuButtons()
        {
            Vector2 margin = new(-32f, -40f);

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                ButtonInfo button = this.menuButtons[i];

                Image backgroundElement = new()
                {
                    Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                    SourceRectangle = new(320, 140, 32, 32),
                    Scale = new(UIConstants.HUD_SLOT_SCALE),
                    Size = new(UIConstants.HUD_GRID_SIZE),
                    Margin = margin,
                };

                Image iconElement = new()
                {
                    Texture = button.IconTexture,
                    Scale = new(1.5f),
                    Size = new(UIConstants.HUD_GRID_SIZE)
                };

                SlotInfo slot = new(backgroundElement, iconElement);

                slot.Background.Alignment = CardinalDirection.Northeast;

                // Update
                this.panelBackgroundElement.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.menuButtonSlots[i] = slot;

                // Spacing
                margin.X -= UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);
            }
        }

        private void BuildItemCatalogSlots()
        {
            Vector2 slotMargin = new(96, 192);

            int index = 0;
            for (int col = 0; col < UIConstants.HUD_ITEM_EXPLORER_ITEMS_PER_COLUMN; col++)
            {
                for (int row = 0; row < UIConstants.HUD_ITEM_EXPLORER_ITEMS_PER_ROW; row++)
                {
                    Image slotBackground = new()
                    {
                        Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                        SourceRectangle = new(320, 140, 32, 32),
                        Scale = new(UIConstants.HUD_ITEM_EXPLORER_SLOT_SCALE),
                        Alignment = CardinalDirection.Northwest,
                        Size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE),
                        Margin = slotMargin
                    };

                    Image slotIcon = new()
                    {
                        Scale = new(1.5f),
                        Size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE)
                    };

                    // Position
                    this.panelBackgroundElement.AddChild(slotBackground);
                    slotBackground.AddChild(slotIcon);

                    // Spacing
                    slotMargin.X += UIConstants.HUD_ITEM_EXPLORER_SLOT_SPACING + (UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE / 2);
                    this.itemButtonSlots[index] = new(slotBackground, slotIcon);
                    index++;
                }

                slotMargin.X = 96;
                slotMargin.Y += UIConstants.HUD_ITEM_EXPLORER_SLOT_SPACING + (UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE / 2);
            }
        }

        private void BuildCategoryButtons()
        {
            Vector2 slotMargin = new(32f, -40f);

            int index = 0;

            for (int i = 0; i < CatalogDatabase.Categories.Length; i++)
            {
                Category category = CatalogDatabase.Categories[i];

                Image slotBackground = new()
                {
                    Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                    SourceRectangle = new(320, 140, 32, 32),
                    Alignment = CardinalDirection.Northwest,
                    Scale = new(UIConstants.HUD_ITEM_EXPLORER_SLOT_SCALE),
                    Size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE),
                    Margin = slotMargin
                };

                Image slotIcon = new()
                {
                    Texture = category.IconTexture,
                    Scale = new(1.5f),
                    Size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE)
                };

                // Data
                if (!slotBackground.ContainsData(UIConstants.DATA_CATEGORY))
                {
                    slotBackground.AddData(UIConstants.DATA_CATEGORY, category);
                }

                // Position
                this.panelBackgroundElement.AddChild(slotBackground);
                slotBackground.AddChild(slotIcon);

                // Spacing
                slotMargin.X += UIConstants.HUD_ITEM_EXPLORER_SLOT_SPACING + (UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE / 2);
                this.categoryButtonSlots[index] = new(slotBackground, slotIcon);
                index++;
            }
        }

        private void BuildSubcategoryButtons()
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
                    Image slotBackground = new()
                    {
                        Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                        SourceRectangle = new(320, 140, 32, 32),
                        Alignment = positionAnchor,
                        Scale = new(UIConstants.HUD_ITEM_EXPLORER_SLOT_SCALE),
                        Size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE),
                        Margin = margin
                    };

                    Image slotIcon = new()
                    {
                        Scale = new(1.5f),
                        Size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE)
                    };

                    // Position
                    this.panelBackgroundElement.AddChild(slotBackground);
                    slotBackground.AddChild(slotIcon);

                    // Spacing
                    margin.Y += UIConstants.HUD_ITEM_EXPLORER_SLOT_SPACING + (UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE / 2);
                    this.subcategoryButtonSlots[index] = new(slotBackground, slotIcon);
                    index++;
                }
            }
        }

        private void BuildPagination(Container root)
        {
            this.pageIndexLabelElement = new()
            {
                Scale = new(0.1f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = CardinalDirection.South,
                Margin = new(0f, -35f),
                TextContent = "1 / 1",

                BorderDirections = LabelBorderDirection.All,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderOffset = 2f,
                BorderThickness = 2f,
            };

            this.panelBackgroundElement.AddChild(this.pageIndexLabelElement);

            for (int i = 0; i < this.paginationButtons.Length; i++)
            {
                Image slotBackground = new()
                {
                    Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                    SourceRectangle = new(320, 140, 32, 32),
                    Scale = new(1.5f),
                    Size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE),
                };

                Image slotIcon = new()
                {
                    Texture = this.paginationButtons[i].IconTexture,
                    Scale = new(1f),
                    Size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE)
                };

                slotBackground.AddChild(slotIcon);

                // Spacing
                this.paginationButtonSlots[i] = new(slotBackground, slotIcon);

                // Adding
                root.AddChild(slotBackground);
                root.AddChild(slotIcon);
            }

            // Left
            SlotInfo leftSlot = this.paginationButtonSlots[0];
            leftSlot.Background.Alignment = CardinalDirection.Southwest;
            leftSlot.Background.Margin = new(34f, -34f);

            // Right
            SlotInfo rightSlot = this.paginationButtonSlots[1];
            rightSlot.Background.Alignment = CardinalDirection.Southeast;
            rightSlot.Background.Margin = new(-34f);

            foreach (SlotInfo slot in this.paginationButtonSlots)
            {
                this.panelBackgroundElement.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);
            }
        }

        #endregion

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.tooltipBox.CanDraw = false;

            UpdateMenuButtons();
            UpdateCategoryButtons();
            UpdateSubcategoryButtons();
            UpdateItemCatalog();
            UpdatePagination();

            this.tooltipBox.RefreshDisplay(TooltipBoxContent.Title, TooltipBoxContent.Description);
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                SlotInfo slot = this.menuButtonSlots[i];

                Vector2 position = slot.Background.Position;
                Vector2 size = new(UIConstants.HUD_GRID_SIZE);

                if (Interaction.OnMouseClick(position, size))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                if (Interaction.OnMouseOver(position, size))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.Title = this.menuButtons[i].Name;
                    TooltipBoxContent.Description = this.menuButtons[i].Description;

                    slot.Background.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.Background.Color = AAP64ColorPalette.White;
                }
            }
        }

        private void UpdateCategoryButtons()
        {
            for (int i = 0; i < this.categoryButtonSlots.Length; i++)
            {
                SlotInfo categorySlot = this.categoryButtonSlots[i];

                if (!categorySlot.Background.CanDraw)
                {
                    continue;
                }

                Category category = (Category)categorySlot.Background.GetData(UIConstants.DATA_CATEGORY);

                Vector2 position = categorySlot.Background.Position;
                Vector2 size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE);

                // Check if the mouse clicked on the current slot.
                if (Interaction.OnMouseClick(position, size))
                {
                    SelectItemCatalog(category, category.Subcategories[0], 0);
                }

                bool isOver = Interaction.OnMouseOver(position, size);

                if (isOver)
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.Title = category.Name;
                    TooltipBoxContent.Description = category.Description;
                }
            }
        }

        private void UpdateSubcategoryButtons()
        {
            for (int i = 0; i < this.subcategoryButtonSlots.Length; i++)
            {
                SlotInfo subcategorySlot = this.subcategoryButtonSlots[i];

                if (!subcategorySlot.Background.CanDraw)
                {
                    continue;
                }

                Subcategory subcategory = (Subcategory)subcategorySlot.Background.GetData(UIConstants.DATA_SUBCATEGORY);

                Vector2 position = subcategorySlot.Background.Position;
                Vector2 size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE);

                // Check if the mouse clicked on the current slot.
                if (Interaction.OnMouseClick(position, size))
                {
                    SelectItemCatalog(subcategory.ParentCategory, subcategory, 0);
                }

                bool isOver = Interaction.OnMouseOver(position, size);

                if (isOver)
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.Title = subcategory.Name;
                    TooltipBoxContent.Description = subcategory.Description;
                }
            }
        }

        private void UpdateItemCatalog()
        {
            for (int i = 0; i < this.itemButtonSlots.Length; i++)
            {
                SlotInfo slot = this.itemButtonSlots[i];

                if (!slot.Background.CanDraw)
                {
                    continue;
                }

                Item item = (Item)slot.Background.GetData(UIConstants.DATA_ITEM);

                Vector2 position = slot.Background.Position;
                Vector2 size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE);

                if (Interaction.OnMouseClick(position, size))
                {
                    this.hudUI.AddItemToToolbar(item);
                    this.uiManager.CloseGUI();
                }

                bool isOver = Interaction.OnMouseOver(position, size);

                if (isOver)
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.Title = item.Name;
                    TooltipBoxContent.Description = item.Description;

                    slot.Background.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.Background.Color = AAP64ColorPalette.White;
                }

                if (this.hudUI.ItemIsEquipped(item))
                {
                    slot.Background.Color = AAP64ColorPalette.TealGray;
                }
            }
        }

        private void UpdatePagination()
        {
            for (int i = 0; i < this.paginationButtons.Length; i++)
            {
                SlotInfo slot = this.paginationButtonSlots[i];

                Vector2 position = slot.Background.Position;
                Vector2 size = new(UIConstants.HUD_ITEM_EXPLORER_GRID_SIZE);

                if (Interaction.OnMouseClick(position, size))
                {
                    this.paginationButtons[i].ClickAction?.Invoke();
                }

                slot.Background.Color = Interaction.OnMouseOver(position, size) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        #endregion

        #region UTILITIES

        private void SelectItemCatalog(Category category, Subcategory subcategory, int pageIndex)
        {
            this.menuTitleElement.TextContent = subcategory.Name;

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
                SlotInfo subcategorySlot = this.subcategoryButtonSlots[i];

                if (i < this.selectedCategory.SubcategoriesLength)
                {
                    subcategorySlot.Show();

                    Subcategory subcategory = this.selectedCategory.Subcategories[i];
                    subcategorySlot.Icon.Texture = subcategory.IconTexture;

                    // Add or Update Data
                    if (!subcategorySlot.Background.ContainsData(UIConstants.DATA_SUBCATEGORY))
                    {
                        subcategorySlot.Background.AddData(UIConstants.DATA_SUBCATEGORY, subcategory);
                    }
                    else
                    {
                        subcategorySlot.Background.UpdateData(UIConstants.DATA_SUBCATEGORY, subcategory);
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
            this.pageIndexLabelElement.TextContent = string.Concat(this.currentPage + 1, " / ", this.totalPages + 1);

            int startIndex = this.currentPage * UIConstants.HUD_ITEM_EXPLORER_ITEMS_PER_PAGE;
            int endIndex = Math.Min(startIndex + UIConstants.HUD_ITEM_EXPLORER_ITEMS_PER_PAGE, this.selectedSubcategory.ItemLength);

            this.selectedItems = this.selectedSubcategory.GetItems(startIndex, endIndex);
            this.selectedItemsLength = this.selectedItems.Length;

            for (int i = 0; i < this.itemButtonSlots.Length; i++)
            {
                SlotInfo itemSlot = this.itemButtonSlots[i];

                if (i < this.selectedItemsLength)
                {
                    itemSlot.Show();

                    Item item = this.selectedItems[i];

                    if (item == null)
                    {
                        continue;
                    }

                    itemSlot.Icon.Texture = item.IconTexture;

                    // Add or Update Data
                    if (!itemSlot.Background.ContainsData(UIConstants.DATA_ITEM))
                    {
                        itemSlot.Background.AddData(UIConstants.DATA_ITEM, item);
                    }
                    else
                    {
                        itemSlot.Background.UpdateData(UIConstants.DATA_ITEM, item);
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
