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

        private Image background;

        private Label menuTitle;
        private Label pageIndexLabel;

        private readonly TooltipBox tooltipBox;

        private readonly ButtonInfo[] menuButtonInfos;
        private readonly ButtonInfo[] paginationButtonInfos;

        private readonly SlotInfo[] menuButtonSlotInfos;
        private readonly SlotInfo[] itemButtonSlotInfos;
        private readonly SlotInfo[] categoryButtonSlotInfos;
        private readonly SlotInfo[] subcategoryButtonSlotInfos;
        private readonly SlotInfo[] paginationButtonSlotInfos;

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

            this.menuButtonInfos = [
                new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, this.uiManager.CloseGUI),
            ];

            this.paginationButtonInfos = [
                new(TextureIndex.IconUI, new(128, 160, 32, 32), "Left", string.Empty, () =>
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
                }),

                new(TextureIndex.IconUI, new(64, 160, 32, 32), "Right", string.Empty, () =>
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
                }),
            ];

            this.menuButtonSlotInfos = new SlotInfo[this.menuButtonInfos.Length];
            this.itemButtonSlotInfos = new SlotInfo[UIConstants.HUD_ITEM_EXPLORER_ITEMS_PER_PAGE];
            this.categoryButtonSlotInfos = new SlotInfo[CatalogDatabase.CategoryLength];
            this.subcategoryButtonSlotInfos = new SlotInfo[UIConstants.HUD_ITEM_EXPLORER_SUB_CATEGORY_BUTTONS_LENGTH];
            this.paginationButtonSlotInfos = new SlotInfo[this.paginationButtonInfos.Length];
        }

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
                Size = new(1.0f),
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            this.background = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.UIBackgroundItemExplorer),
                Size = new(1084.0f, 607.0f),
                Margin = new(98.0f, 90.0f),
            };

            root.AddChild(backgroundShadowElement);
            root.AddChild(this.background);
        }

        private void BuildTitle()
        {
            this.menuTitle = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.12f),
                Alignment = CardinalDirection.Northwest,
                Margin = new(32.0f, 40.0f),
                Color = AAP64ColorPalette.White,
                TextContent = Localization_GUIs.HUD_Complements_ItemExplorer_Title,

                BorderDirections = LabelBorderDirection.All,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderOffset = 3.0f,
                BorderThickness = 3.0f,
            };

            this.background.AddChild(this.menuTitle);
        }

        private void BuildMenuButtons()
        {
            float marginX = -32.0f;

            for (byte i = 0; i < this.menuButtonInfos.Length; i++)
            {
                ButtonInfo button = this.menuButtonInfos[i];

                Image backgroundElement = new()
                {
                    Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                    SourceRectangle = new(320, 140, 32, 32),
                    Scale = new(2.0f),
                    Size = new(32.0f),
                    Margin = new(marginX, -40.0f),
                };

                Image iconElement = new()
                {
                    Texture = button.Texture,
                    SourceRectangle = button.TextureSourceRectangle,
                    Scale = new(1.5f),
                    Size = new(32.0f)
                };

                SlotInfo slot = new(backgroundElement, iconElement);

                slot.Background.Alignment = CardinalDirection.Northeast;

                // Update
                this.background.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.menuButtonSlotInfos[i] = slot;

                // Spacing
                marginX -= 80.0f;
            }
        }

        private void BuildItemCatalogSlots()
        {
            Vector2 slotMargin = new(96.0f, 192.0f);

            int index = 0;

            for (byte col = 0; col < UIConstants.HUD_ITEM_EXPLORER_ITEMS_PER_COLUMN; col++)
            {
                for (byte row = 0; row < UIConstants.HUD_ITEM_EXPLORER_ITEMS_PER_ROW; row++)
                {
                    Image slotBackground = new()
                    {
                        Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                        SourceRectangle = new(320, 140, 32, 32),
                        Scale = new(2.0f),
                        Alignment = CardinalDirection.Northwest,
                        Size = new(32.0f),
                        Margin = slotMargin
                    };

                    Image slotIcon = new()
                    {
                        Scale = new(1.5f),
                        Size = new(32.0f)
                    };

                    // Position
                    this.background.AddChild(slotBackground);
                    slotBackground.AddChild(slotIcon);

                    // Spacing
                    slotMargin.X += 80.0f;
                    this.itemButtonSlotInfos[index] = new(slotBackground, slotIcon);
                    index++;
                }

                slotMargin.X = 96.0f;
                slotMargin.Y += 80.0f;
            }
        }

        private void BuildCategoryButtons()
        {
            Vector2 slotMargin = new(32.0f, -40.0f);

            int index = 0;

            for (byte i = 0; i < CatalogDatabase.Categories.Length; i++)
            {
                Category category = CatalogDatabase.Categories[i];

                Image slotBackground = new()
                {
                    Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                    SourceRectangle = new(320, 140, 32, 32),
                    Alignment = CardinalDirection.Northwest,
                    Scale = new(2.0f),
                    Size = new(32.0f),
                    Margin = slotMargin
                };

                Image slotIcon = new()
                {
                    Texture = category.Texture,
                    SourceRectangle = category.TextureSourceRectangle,
                    Scale = new(1.5f),
                    Size = new(32.0f)
                };

                // Data
                if (!slotBackground.ContainsData(UIConstants.DATA_CATEGORY))
                {
                    slotBackground.AddData(UIConstants.DATA_CATEGORY, category);
                }

                // Position
                this.background.AddChild(slotBackground);
                slotBackground.AddChild(slotIcon);

                // Spacing
                slotMargin.X += 80.0f;
                this.categoryButtonSlotInfos[index] = new(slotBackground, slotIcon);

                index++;
            }
        }

        private void BuildSubcategoryButtons()
        {
            int index = 0;
            int sideCounts = UIConstants.HUD_ITEM_EXPLORER_SUB_CATEGORY_BUTTONS_LENGTH / 2;
            Vector2 margin;

            margin = new(-48.0f, 48.0f);
            BuildSlots(CardinalDirection.Northwest);

            margin = new(48.0f, 48.0f);
            BuildSlots(CardinalDirection.Northeast);

            // =============================== //

            void BuildSlots(CardinalDirection positionAnchor)
            {
                for (byte i = 0; i < sideCounts; i++)
                {
                    Image slotBackground = new()
                    {
                        Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                        SourceRectangle = new(320, 140, 32, 32),
                        Alignment = positionAnchor,
                        Scale = new(2.0f),
                        Size = new(32.0f),
                        Margin = margin
                    };

                    Image slotIcon = new()
                    {
                        Scale = new(1.5f),
                        Size = new(32.0f)
                    };

                    // Position
                    this.background.AddChild(slotBackground);
                    slotBackground.AddChild(slotIcon);

                    // Spacing
                    margin.Y += 80.0f;
                    this.subcategoryButtonSlotInfos[index] = new(slotBackground, slotIcon);
                    index++;
                }
            }
        }

        private void BuildPagination(Container root)
        {
            this.pageIndexLabel = new()
            {
                Scale = new(0.1f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = CardinalDirection.South,
                Margin = new(0.0f, -35.0f),
                TextContent = "1 / 1",

                BorderDirections = LabelBorderDirection.All,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
            };

            this.background.AddChild(this.pageIndexLabel);

            for (byte i = 0; i < this.paginationButtonInfos.Length; i++)
            {
                Image slotBackground = new()
                {
                    Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                    SourceRectangle = new(320, 140, 32, 32),
                    Scale = new(1.5f),
                    Size = new(32.0f),
                };

                Image slotIcon = new()
                {
                    Texture = this.paginationButtonInfos[i].Texture,
                    SourceRectangle = this.paginationButtonInfos[i].TextureSourceRectangle,
                    Scale = new(1.0f),
                    Size = new(32.0f)
                };

                slotBackground.AddChild(slotIcon);

                // Spacing
                this.paginationButtonSlotInfos[i] = new(slotBackground, slotIcon);

                // Adding
                root.AddChild(slotBackground);
                root.AddChild(slotIcon);
            }

            // Left
            SlotInfo leftSlot = this.paginationButtonSlotInfos[0];
            leftSlot.Background.Alignment = CardinalDirection.Southwest;
            leftSlot.Background.Margin = new(34.0f, -34.0f);

            // Right
            SlotInfo rightSlot = this.paginationButtonSlotInfos[1];
            rightSlot.Background.Alignment = CardinalDirection.Southeast;
            rightSlot.Background.Margin = new(-34.0f);

            foreach (SlotInfo slot in this.paginationButtonSlotInfos)
            {
                this.background.AddChild(slot.Background);
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
        }

        private void UpdateMenuButtons()
        {
            for (byte i = 0; i < this.menuButtonInfos.Length; i++)
            {
                SlotInfo slot = this.menuButtonSlotInfos[i];

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    this.menuButtonInfos[i].ClickAction?.Invoke();
                }

                if (Interaction.OnMouseOver(slot.Background))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.menuButtonInfos[i].Name);
                    TooltipBoxContent.SetDescription(this.menuButtonInfos[i].Description);

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
            for (byte i = 0; i < this.categoryButtonSlotInfos.Length; i++)
            {
                SlotInfo categorySlot = this.categoryButtonSlotInfos[i];

                if (!categorySlot.Background.CanDraw)
                {
                    continue;
                }

                Category category = (Category)categorySlot.Background.GetData(UIConstants.DATA_CATEGORY);

                if (Interaction.OnMouseLeftClick(categorySlot.Background))
                {
                    SelectItemCatalog(category, category.Subcategories[0], 0);
                }

                if (Interaction.OnMouseOver(categorySlot.Background))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(category.Name);
                    TooltipBoxContent.SetDescription(category.Description);
                }
            }
        }

        private void UpdateSubcategoryButtons()
        {
            for (byte i = 0; i < this.subcategoryButtonSlotInfos.Length; i++)
            {
                SlotInfo subcategorySlot = this.subcategoryButtonSlotInfos[i];

                if (!subcategorySlot.Background.CanDraw)
                {
                    continue;
                }

                Subcategory subcategory = (Subcategory)subcategorySlot.Background.GetData(UIConstants.DATA_SUBCATEGORY);

                if (Interaction.OnMouseLeftClick(subcategorySlot.Background))
                {
                    SelectItemCatalog(subcategory.ParentCategory, subcategory, 0);
                }

                if (Interaction.OnMouseOver(subcategorySlot.Background))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(subcategory.Name);
                    TooltipBoxContent.SetDescription(subcategory.Description);
                }
            }
        }

        private void UpdateItemCatalog()
        {
            for (byte i = 0; i < this.itemButtonSlotInfos.Length; i++)
            {
                SlotInfo slot = this.itemButtonSlotInfos[i];

                if (!slot.Background.CanDraw)
                {
                    continue;
                }

                Item item = (Item)slot.Background.GetData(UIConstants.DATA_ITEM);

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    this.hudUI.AddItemToToolbar(item);
                    this.uiManager.CloseGUI();
                }

                if (Interaction.OnMouseOver(slot.Background))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(item.Name);
                    TooltipBoxContent.SetDescription(item.Description);

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
            for (byte i = 0; i < this.paginationButtonInfos.Length; i++)
            {
                SlotInfo slot = this.paginationButtonSlotInfos[i];

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    this.paginationButtonInfos[i].ClickAction?.Invoke();
                }

                slot.Background.Color = Interaction.OnMouseOver(slot.Background) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        #endregion

        #region UTILITIES

        private void SelectItemCatalog(Category category, Subcategory subcategory, int pageIndex)
        {
            this.menuTitle.TextContent = subcategory.Name;

            this.selectedCategory = category;
            this.selectedSubcategory = subcategory;
            this.currentPage = pageIndex;
            this.totalPages = subcategory.Items.Length / UIConstants.HUD_ITEM_EXPLORER_ITEMS_PER_PAGE;

            ChangeSubcategorCatalog();
            ChangeItemCatalog();
        }

        private void ChangeSubcategorCatalog()
        {
            for (byte i = 0; i < this.subcategoryButtonSlotInfos.Length; i++)
            {
                SlotInfo subcategorySlot = this.subcategoryButtonSlotInfos[i];

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
            this.pageIndexLabel.TextContent = string.Concat(this.currentPage + 1, " / ", this.totalPages + 1);

            int startIndex = this.currentPage * UIConstants.HUD_ITEM_EXPLORER_ITEMS_PER_PAGE;
            int endIndex = Math.Min(startIndex + UIConstants.HUD_ITEM_EXPLORER_ITEMS_PER_PAGE, this.selectedSubcategory.ItemLength);

            this.selectedItems = this.selectedSubcategory.GetItems(startIndex, endIndex);
            this.selectedItemsLength = this.selectedItems.Length;

            for (byte i = 0; i < this.itemButtonSlotInfos.Length; i++)
            {
                SlotInfo itemSlot = this.itemButtonSlotInfos[i];

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
