/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;

using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Catalog;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.States;
using StardustSandbox.Core.Enums.UI;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Information;

using System;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed class ItemExplorerUI : UIBase
    {
        private Category selectedCategory;
        private Subcategory selectedSubcategory;

        private int currentPageIndex = 0, totalPages = 0, selectedSubcategoriesLength = 0, selectedItemsLength = 0;

        private Subcategory[] selectedSubcategories;
        private Item[] selectedItems;

        private Image panelBackground, shadowBackground;
        private Label menuTitle, pageIndexLabel;
        private SlotInfo[] menuButtonSlotInfos;

        private readonly TooltipBox tooltipBox;

        private readonly ButtonInfo[] buttonInfos, paginationButtonInfos;
        private readonly SlotInfo[] itemButtonSlotInfos, categoryButtonSlotInfos, subcategoryButtonSlotInfos, paginationButtonSlotInfos;

        private readonly HudUI hudUI;
        private readonly ItemSearchUI itemSearchUI;

        private readonly UIManager uiManager;

        internal ItemExplorerUI(
            HudUI hudUI,
            ItemSearchUI itemSearchUI,
            TooltipBox tooltipBox,
            UIManager uiManager
        ) : base()
        {
            this.uiManager = uiManager;
            this.hudUI = hudUI;
            this.itemSearchUI = itemSearchUI;
            this.tooltipBox = tooltipBox;

            this.buttonInfos = [
                new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, uiManager.CloseUI),
                new(TextureIndex.IconUI, new(0, 0, 32, 32), "Search", string.Empty, () =>
                {
                    this.itemSearchUI.Setup(result => {
                        SoundEngine.Play(SoundEffectIndex.GUI_Accepted);
                        hudUI.AddItemToToolbar(result);
                        uiManager.CloseUI();
                    });

                    uiManager.OpenUI(UIIndex.ItemSearch);
                }),
            ];

            this.paginationButtonInfos = [
                // Left
                new(TextureIndex.IconUI, new(128, 160, 32, 32), string.Empty, string.Empty, () =>
                {
                    if (this.currentPageIndex > 0)
                    {
                        this.currentPageIndex--;
                    }
                    else
                    {
                        this.currentPageIndex = this.totalPages - 1;
                    }

                    RefreshItemCatalog();
                }),

                // Right
                new(TextureIndex.IconUI, new(64, 160, 32, 32), string.Empty, string.Empty, () =>
                {
                    if (this.currentPageIndex < this.totalPages - 1)
                    {
                        this.currentPageIndex++;
                    }
                    else
                    {
                        this.currentPageIndex = 0;
                    }

                    RefreshItemCatalog();
                }),
            ];

            this.itemButtonSlotInfos = new SlotInfo[UIConstants.ITEM_EXPLORER_ITEMS_PER_PAGE];
            this.categoryButtonSlotInfos = new SlotInfo[CatalogDatabase.CategoryLength];
            this.subcategoryButtonSlotInfos = new SlotInfo[UIConstants.ITEM_EXPLORER_SUBCATEGORY_BUTTONS_LENGTH];
            this.paginationButtonSlotInfos = new SlotInfo[this.paginationButtonInfos.Length];
        }

        private void SelectItemCatalog(Category category, Subcategory subcategory, int pageIndex)
        {
            this.menuTitle.TextContent = subcategory.Name;

            this.selectedCategory = category;
            this.selectedSubcategory = subcategory;
            this.currentPageIndex = pageIndex;
            this.totalPages = (int)MathF.Max(1.0f, MathF.Ceiling(subcategory.ItemsLength / (float)UIConstants.ITEM_EXPLORER_ITEMS_PER_PAGE));

            RefreshContent();
        }

        private void SelectItemCatalog(int categoryIndex, int subcategoryIndex, int pageIndex)
        {
            Category category = CatalogDatabase.GetCategory(categoryIndex);
            Subcategory subcategory = category.GetSubcategory(subcategoryIndex);

            SelectItemCatalog(category, subcategory, pageIndex);
        }

        private void RefreshContent()
        {
            RefreshSubcategoryButtons();
            RefreshItemCatalog();
        }

        private void RefreshSubcategoryButtons()
        {
            int subcategoriesCount = this.selectedCategory.SubcategoriesLength;

            this.selectedSubcategories = new Subcategory[subcategoriesCount];
            this.selectedSubcategoriesLength = subcategoriesCount;

            for (int i = 0; i < this.subcategoryButtonSlotInfos.Length; i++)
            {
                SlotInfo subcategorySlot = this.subcategoryButtonSlotInfos[i];

                if (i < subcategoriesCount)
                {
                    subcategorySlot.Background.CanDraw = true;

                    Subcategory subcategory = this.selectedCategory.GetSubcategory(i);
                    this.selectedSubcategories[i] = subcategory;

                    subcategorySlot.Icon.Texture = subcategory.Texture;
                    subcategorySlot.Icon.SourceRectangle = subcategory.SourceRectangle;
                }
                else
                {
                    subcategorySlot.Background.CanDraw = false;
                }
            }
        }

        private void RefreshItemCatalog()
        {
            this.pageIndexLabel.TextContent = string.Concat(this.currentPageIndex + 1, " / ", this.totalPages);

            int startIndex = this.currentPageIndex * UIConstants.ITEM_EXPLORER_ITEMS_PER_PAGE;
            int endIndex = Math.Min(startIndex + UIConstants.ITEM_EXPLORER_ITEMS_PER_PAGE, this.selectedSubcategory.ItemsLength);
            int length = endIndex - startIndex;

            this.selectedItems = this.selectedSubcategory.GetItems(startIndex, endIndex);
            this.selectedItemsLength = length;

            for (int i = 0; i < this.itemButtonSlotInfos.Length; i++)
            {
                SlotInfo itemSlot = this.itemButtonSlotInfos[i];

                if (i < length)
                {
                    itemSlot.Background.CanDraw = true;
                    Item item = this.selectedItems[i];

                    itemSlot.Icon.TextureIndex = item.TextureIndex;
                    itemSlot.Icon.SourceRectangle = item.SourceRectangle;
                }
                else
                {
                    itemSlot.Background.CanDraw = false;
                }
            }
        }

        internal void Setup()
        {
            SelectItemCatalog(0, 0, 0);
        }

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildTitle();
            BuildMenuButtons();
            BuildCategoryButtons();
            BuildSubcategoryButtons();
            BuildItemSlots();
            BuildPagination();

            root.AddChild(this.tooltipBox);
        }

        private void BuildBackground(Container root)
        {
            this.shadowBackground = new()
            {
                TextureIndex = TextureIndex.Pixel,
                Scale = GameScreen.GetViewport(),
                Color = new(AAP64ColorPalette.DarkGray, 160),
                Size = Vector2.One,
            };

            this.panelBackground = new()
            {
                TextureIndex = TextureIndex.UIBackgroundItemExplorer,
                Size = new(1084.0f, 607.0f),
                Margin = new(0.0f, 32.0f),
                Alignment = UIDirection.Center,
            };

            root.AddChild(this.shadowBackground);
            root.AddChild(this.panelBackground);
        }

        private void BuildTitle()
        {
            this.menuTitle = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.12f),
                Margin = new(32.0f, 40.0f),
                Color = AAP64ColorPalette.White,
                TextContent = Localization_GUIs.ItemExplorer_Title,

                BorderDirections = LabelBorderDirection.All,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderOffset = 3.0f,
                BorderThickness = 3.0f,
            };

            this.panelBackground.AddChild(this.menuTitle);
        }

        private void BuildMenuButtons()
        {
            this.menuButtonSlotInfos = UIBuilderUtility.BuildHorizontalButtonLine(
                this.panelBackground,
                this.buttonInfos,
                new(-32.0f, -72.0f),
                -80.0f,
                UIDirection.Northeast
            );
        }

        private void BuildCategoryButtons()
        {
            Vector2 margin = new(32.0f, -72.0f);

            int index = 0;

            for (int i = 0; i < CatalogDatabase.Categories.Length; i++)
            {
                Category category = CatalogDatabase.Categories[i];
                SlotInfo slot = new(
                    new()
                    {
                        TextureIndex = TextureIndex.UIButtons,
                        SourceRectangle = new(320, 140, 32, 32),
                        Alignment = UIDirection.Northwest,
                        Scale = new(2.0f),
                        Size = new(32.0f),
                        Margin = margin
                    },

                    new()
                    {
                        Texture = category.Texture,
                        SourceRectangle = category.SourceRectangle,
                        Alignment = UIDirection.Center,
                        Scale = new(1.5f),
                        Size = new(32.0f)
                    }
                );

                // Position
                this.panelBackground.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Spacing
                margin.X += 80.0f;
                this.categoryButtonSlotInfos[index] = slot;

                index++;
            }
        }

        private void BuildSubcategoryButtons()
        {
            int index = 0;
            int sideCounts = UIConstants.ITEM_EXPLORER_SUBCATEGORY_BUTTONS_LENGTH / 2;
            Vector2 margin;

            margin = new(-80.0f, 32.0f);
            BuildSlots(UIDirection.Northwest);

            margin = new(80.0f, 32.0f);
            BuildSlots(UIDirection.Northeast);

            void BuildSlots(UIDirection positionAnchor)
            {
                for (int i = 0; i < sideCounts; i++)
                {
                    SlotInfo slot = new(
                        new()
                        {
                            TextureIndex = TextureIndex.UIButtons,
                            SourceRectangle = new(320, 140, 32, 32),
                            Alignment = positionAnchor,
                            Scale = new(2.0f),
                            Size = new(32.0f),
                            Margin = margin
                        },

                        new()
                        {
                            TextureIndex = TextureIndex.IconElements,
                            SourceRectangle = new(0, 0, 32, 32),
                            Alignment = UIDirection.Center,
                            Scale = new(1.5f),
                            Size = new(32.0f)
                        }
                    );

                    // Position
                    this.panelBackground.AddChild(slot.Background);
                    slot.Background.AddChild(slot.Icon);

                    // Spacing
                    margin.Y += 80.0f;
                    this.subcategoryButtonSlotInfos[index] = slot;
                    index++;
                }
            }
        }

        private void BuildItemSlots()
        {
            Vector2 margin = new(68.0f, 168.0f);

            int index = 0;

            for (byte col = 0; col < UIConstants.ITEM_EXPLORER_ITEMS_PER_COLUMN; col++)
            {
                for (byte row = 0; row < UIConstants.ITEM_EXPLORER_ITEMS_PER_ROW; row++)
                {
                    SlotInfo slot = new(
                        new()
                        {
                            TextureIndex = TextureIndex.UIButtons,
                            SourceRectangle = new(320, 140, 32, 32),
                            Alignment = UIDirection.Northwest,
                            Scale = new(2.0f),
                            Size = new(32.0f),
                            Margin = margin
                        },

                        new()
                        {
                            Alignment = UIDirection.Center,
                            TextureIndex = TextureIndex.IconElements,
                            SourceRectangle = new(0, 0, 32, 32),
                            Scale = new(1.5f),
                            Size = new(32.0f)
                        }
                    );

                    // Position
                    this.panelBackground.AddChild(slot.Background);
                    slot.Background.AddChild(slot.Icon);

                    // Spacing
                    margin.X += 80.0f;

                    this.itemButtonSlotInfos[index] = slot;
                    index++;
                }

                margin.X = 68.0f;
                margin.Y += 80.0f;
            }
        }

        private void BuildPagination()
        {
            this.pageIndexLabel = new()
            {
                Scale = new(0.1f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = UIDirection.South,
                Margin = new(0.0f, -12.0f),
                TextContent = "1 / 1",

                BorderDirections = LabelBorderDirection.All,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
            };

            this.panelBackground.AddChild(this.pageIndexLabel);

            for (int i = 0; i < this.paginationButtonInfos.Length; i++)
            {
                SlotInfo slot = new(
                    new()
                    {
                        TextureIndex = TextureIndex.UIButtons,
                        SourceRectangle = new(320, 140, 32, 32),
                        Scale = new(1.6f),
                        Size = new(32.0f),
                    },

                    new()
                    {
                        TextureIndex = this.paginationButtonInfos[i].TextureIndex,
                        SourceRectangle = this.paginationButtonInfos[i].TextureSourceRectangle,
                        Alignment = UIDirection.Center,
                        Size = new(32.0f)
                    }
                );

                // Spacing
                this.paginationButtonSlotInfos[i] = slot;

                // Adding
                this.panelBackground.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);
            }

            SlotInfo left = this.paginationButtonSlotInfos[0];
            left.Background.Alignment = UIDirection.Southwest;
            left.Background.Margin = new(9.0f, -9.0f);

            SlotInfo right = this.paginationButtonSlotInfos[1];
            right.Background.Alignment = UIDirection.Southeast;
            right.Background.Margin = new(-9.0f);

            for (int i = 0; i < this.paginationButtonSlotInfos.Length; i++)
            {
                SlotInfo slot = this.paginationButtonSlotInfos[i];

                this.panelBackground.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);
            }
        }

        protected override void OnResize(Vector2 newSize)
        {
            this.shadowBackground.Scale = newSize;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;

            UpdateMenuButtons();
            UpdateCategoryButtons();
            UpdateSubcategoryButtons();
            UpdateItemCatalog();
            UpdatePagination();
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.buttonInfos.Length; i++)
            {
                SlotInfo slot = this.menuButtonSlotInfos[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    this.buttonInfos[i].ClickAction?.Invoke();
                    break;
                }

                if (Interaction.OnMouseOver(slot.Background))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.buttonInfos[i].Name);
                    TooltipBoxContent.SetDescription(this.buttonInfos[i].Description);

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
            for (int i = 0; i < this.categoryButtonSlotInfos.Length; i++)
            {
                SlotInfo categorySlot = this.categoryButtonSlotInfos[i];
                Category category = CatalogDatabase.GetCategory(i);

                if (Interaction.OnMouseEnter(categorySlot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(categorySlot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    SelectItemCatalog(category, category.GetSubcategory(0), 0);
                    break;
                }

                if (Interaction.OnMouseOver(categorySlot.Background))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(category.Name);
                    TooltipBoxContent.SetDescription(category.Description);
                }

                categorySlot.Background.Color = this.selectedCategory == category ? AAP64ColorPalette.TealGray : AAP64ColorPalette.White;
            }
        }

        private void UpdateSubcategoryButtons()
        {
            for (int i = 0; i < this.selectedSubcategoriesLength; i++)
            {
                SlotInfo subcategorySlot = this.subcategoryButtonSlotInfos[i];
                Subcategory subcategory = this.selectedSubcategories[i];

                if (Interaction.OnMouseEnter(subcategorySlot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(subcategorySlot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    SelectItemCatalog(subcategory.ParentCategory, subcategory, 0);
                    break;
                }

                if (Interaction.OnMouseOver(subcategorySlot.Background))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(subcategory.Name);
                    TooltipBoxContent.SetDescription(subcategory.Description);
                }

                subcategorySlot.Background.Color = this.selectedSubcategory == this.selectedCategory.GetSubcategory(i) ? AAP64ColorPalette.TealGray : AAP64ColorPalette.White;
            }
        }

        private void UpdateItemCatalog()
        {
            for (int i = 0; i < this.selectedItemsLength; i++)
            {
                SlotInfo slot = this.itemButtonSlotInfos[i];
                Item item = this.selectedItems[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Accepted);
                    this.hudUI.AddItemToToolbar(item);
                    this.uiManager.CloseUI();
                    break;
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
                    slot.Background.Color = this.hudUI.ItemIsEquipped(item) ? AAP64ColorPalette.TealGray : AAP64ColorPalette.White;
                }
            }
        }

        private void UpdatePagination()
        {
            for (int i = 0; i < this.paginationButtonInfos.Length; i++)
            {
                SlotInfo slot = this.paginationButtonSlotInfos[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    this.paginationButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                slot.Background.Color = Interaction.OnMouseOver(slot.Background) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        protected override void OnOpened()
        {
            GameHandler.SetState(GameStates.IsCriticalMenuOpen);
        }

        protected override void OnClosed()
        {
            GameHandler.RemoveState(GameStates.IsCriticalMenuOpen);
        }
    }
}

