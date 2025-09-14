using Microsoft.Xna.Framework;

using StardustSandbox.Core.Catalog;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUISystem;
using StardustSandbox.Core.Constants.GUISystem.GUIs.Hud;
using StardustSandbox.Core.Constants.GUISystem.GUIs.Hud.Complements;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Common.Elements.Graphics;
using StardustSandbox.Core.GUISystem.Common.Elements.Textual;
using StardustSandbox.Core.GUISystem.Common.Helpers.General;
using StardustSandbox.Core.GUISystem.Common.Helpers.Interactive;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Localization.GUIs;

namespace StardustSandbox.Core.GUISystem.GUIs.Hud.Complements.ItemExplorer
{
    internal sealed partial class SGUI_ItemExplorer
    {
        private SGUIImageElement panelBackgroundElement;

        private SGUILabelElement menuTitleElement;
        private SGUILabelElement pageIndexLabelElement;

        private readonly SSlot[] menuButtonSlots;
        private readonly SSlot[] itemButtonSlots;
        private readonly SSlot[] categoryButtonSlots;
        private readonly SSlot[] subcategoryButtonSlots;
        private readonly SSlot[] paginationButtonSlots;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildBackground(layoutBuilder);
            BuildTitle(layoutBuilder);
            BuildMenuButtons(layoutBuilder);
            BuildItemCatalogSlots(layoutBuilder);
            BuildCategoryButtons(layoutBuilder);
            BuildSubcategoryButtons(layoutBuilder);
            BuildPagination(layoutBuilder);

            layoutBuilder.AddElement(this.tooltipBoxElement);
        }

        private void BuildBackground(ISGUILayoutBuilder layoutBuilder)
        {
            SGUIImageElement backgroundShadowElement = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Scale = new(SScreenConstants.DEFAULT_SCREEN_WIDTH, SScreenConstants.DEFAULT_SCREEN_HEIGHT),
                Size = new(1),
                Color = new(SColorPalette.DarkGray, 160)
            };

            this.panelBackgroundElement = new(this.SGameInstance)
            {
                Texture = this.panelBackgroundTexture,
                Size = new(1084, 607),
                Margin = new(98, 90),
            };

            this.panelBackgroundElement.PositionRelativeToScreen();

            layoutBuilder.AddElement(backgroundShadowElement);
            layoutBuilder.AddElement(this.panelBackgroundElement);
        }

        private void BuildTitle(ISGUILayoutBuilder layoutBuilder)
        {
            this.menuTitleElement = new(this.SGameInstance)
            {
                SpriteFont = this.bigApple3PMSpriteFont,
                Scale = new(0.12f),
                PositionAnchor = SCardinalDirection.Northwest,
                OriginPivot = SCardinalDirection.East,
                Margin = new(32, 40),
                Color = SColorPalette.White,
            };

            this.menuTitleElement.SetTextualContent(SLocalization_GUIs.HUD_Complements_ItemExplorer_Title);
            this.menuTitleElement.SetAllBorders(true, SColorPalette.DarkGray, new(3f));
            this.menuTitleElement.PositionRelativeToElement(this.panelBackgroundElement);

            layoutBuilder.AddElement(this.menuTitleElement);
        }

        private void BuildMenuButtons(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 margin = new(-32f, -40f);

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                SButton button = this.menuButtons[i];

                SGUIImageElement backgroundElement = new(this.SGameInstance)
                {
                    Texture = this.guiSmallButtonTexture,
                    Scale = new(SGUI_HUDConstants.SLOT_SCALE),
                    Size = new(SGUI_HUDConstants.GRID_SIZE),
                    Margin = margin,
                };

                SGUIImageElement iconElement = new(this.SGameInstance)
                {
                    Texture = button.IconTexture,
                    OriginPivot = SCardinalDirection.Center,
                    Scale = new(1.5f),
                    Size = new(SGUI_HUDConstants.GRID_SIZE)
                };

                SSlot slot = new(backgroundElement, iconElement);

                slot.BackgroundElement.PositionAnchor = SCardinalDirection.Northeast;
                slot.BackgroundElement.OriginPivot = SCardinalDirection.Center;

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.panelBackgroundElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.menuButtonSlots[i] = slot;

                // Spacing
                margin.X -= SGUI_HUDConstants.SLOT_SPACING + (SGUI_HUDConstants.GRID_SIZE / 2);

                layoutBuilder.AddElement(backgroundElement);
                layoutBuilder.AddElement(iconElement);
            }
        }

        private void BuildItemCatalogSlots(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 slotMargin = new(96, 192);

            int index = 0;
            for (int col = 0; col < SGUI_ItemExplorerConstants.ITEMS_PER_COLUMN; col++)
            {
                for (int row = 0; row < SGUI_ItemExplorerConstants.ITEMS_PER_ROW; row++)
                {
                    SGUIImageElement slotBackground = new(this.SGameInstance)
                    {
                        Texture = this.guiSmallButtonTexture,
                        OriginPivot = SCardinalDirection.Center,
                        Scale = new(SGUI_ItemExplorerConstants.SLOT_SCALE),
                        PositionAnchor = SCardinalDirection.Northwest,
                        Size = new(SGUI_ItemExplorerConstants.GRID_SIZE),
                        Margin = slotMargin
                    };

                    SGUIImageElement slotIcon = new(this.SGameInstance)
                    {
                        OriginPivot = SCardinalDirection.Center,
                        Scale = new(1.5f),
                        Size = new(SGUI_ItemExplorerConstants.GRID_SIZE)
                    };

                    // Position
                    slotBackground.PositionRelativeToElement(this.panelBackgroundElement);
                    slotIcon.PositionRelativeToElement(slotBackground);

                    // Spacing
                    slotMargin.X += SGUI_ItemExplorerConstants.SLOT_SPACING + (SGUI_ItemExplorerConstants.GRID_SIZE / 2);
                    this.itemButtonSlots[index] = new(slotBackground, slotIcon);
                    index++;

                    // Adding
                    layoutBuilder.AddElement(slotBackground);
                    layoutBuilder.AddElement(slotIcon);
                }

                slotMargin.X = 96;
                slotMargin.Y += SGUI_ItemExplorerConstants.SLOT_SPACING + (SGUI_ItemExplorerConstants.GRID_SIZE / 2);
            }
        }

        private void BuildCategoryButtons(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 slotMargin = new(32f, -40f);

            int index = 0;

            foreach (SCategory category in this.SGameInstance.CatalogDatabase.Categories)
            {
                SGUIImageElement slotBackground = new(this.SGameInstance)
                {
                    Texture = this.guiSmallButtonTexture,
                    OriginPivot = SCardinalDirection.Center,
                    PositionAnchor = SCardinalDirection.Northwest,
                    Scale = new(SGUI_ItemExplorerConstants.SLOT_SCALE),
                    Size = new(SGUI_ItemExplorerConstants.GRID_SIZE),
                    Margin = slotMargin
                };

                SGUIImageElement slotIcon = new(this.SGameInstance)
                {
                    Texture = category.IconTexture,
                    OriginPivot = SCardinalDirection.Center,
                    Scale = new(1.5f),
                    Size = new(SGUI_ItemExplorerConstants.GRID_SIZE)
                };

                // Data
                if (!slotBackground.ContainsData(SGUIConstants.DATA_CATEGORY))
                {
                    slotBackground.AddData(SGUIConstants.DATA_CATEGORY, category);
                }

                // Position
                slotBackground.PositionRelativeToElement(this.panelBackgroundElement);
                slotIcon.PositionRelativeToElement(slotBackground);

                // Spacing
                slotMargin.X += SGUI_ItemExplorerConstants.SLOT_SPACING + (SGUI_ItemExplorerConstants.GRID_SIZE / 2);
                this.categoryButtonSlots[index] = new(slotBackground, slotIcon);
                index++;

                // Adding
                layoutBuilder.AddElement(slotBackground);
                layoutBuilder.AddElement(slotIcon);
            }
        }

        private void BuildSubcategoryButtons(ISGUILayoutBuilder layoutBuilder)
        {
            int index = 0;
            int sideCounts = SGUI_ItemExplorerConstants.SUB_CATEGORY_BUTTONS_LENGTH / 2;

            Vector2 margin = new(-48, 48);
            BuildSlots(SCardinalDirection.Northwest);
            margin = new(48, 48);
            BuildSlots(SCardinalDirection.Northeast);

            // =============================== //

            void BuildSlots(SCardinalDirection positionAnchor)
            {
                for (int i = 0; i < sideCounts; i++)
                {
                    SGUIImageElement slotBackground = new(this.SGameInstance)
                    {
                        Texture = this.guiSmallButtonTexture,
                        PositionAnchor = positionAnchor,
                        OriginPivot = SCardinalDirection.Center,
                        Scale = new(SGUI_ItemExplorerConstants.SLOT_SCALE),
                        Size = new(SGUI_ItemExplorerConstants.GRID_SIZE),
                        Margin = margin
                    };

                    SGUIImageElement slotIcon = new(this.SGameInstance)
                    {
                        OriginPivot = SCardinalDirection.Center,
                        Scale = new(1.5f),
                        Size = new(SGUI_ItemExplorerConstants.GRID_SIZE)
                    };

                    // Position
                    slotBackground.PositionRelativeToElement(this.panelBackgroundElement);
                    slotIcon.PositionRelativeToElement(slotBackground);

                    // Spacing
                    margin.Y += SGUI_ItemExplorerConstants.SLOT_SPACING + (SGUI_ItemExplorerConstants.GRID_SIZE / 2);
                    this.subcategoryButtonSlots[index] = new(slotBackground, slotIcon);
                    index++;

                    // Adding
                    layoutBuilder.AddElement(slotBackground);
                    layoutBuilder.AddElement(slotIcon);
                }
            }
        }

        private void BuildPagination(ISGUILayoutBuilder layoutBuilder)
        {
            this.pageIndexLabelElement = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                SpriteFont = this.bigApple3PMSpriteFont,
                PositionAnchor = SCardinalDirection.South,
                OriginPivot = SCardinalDirection.Center,
                Margin = new(0f, -35f),
            };

            this.pageIndexLabelElement.SetTextualContent("1 / 1");
            this.pageIndexLabelElement.SetAllBorders(true, SColorPalette.DarkGray, new(2f));
            this.pageIndexLabelElement.PositionRelativeToElement(this.panelBackgroundElement);

            layoutBuilder.AddElement(this.pageIndexLabelElement);

            for (int i = 0; i < this.paginationButtons.Length; i++)
            {
                SGUIImageElement slotBackground = new(this.SGameInstance)
                {
                    Texture = this.guiSmallButtonTexture,
                    OriginPivot = SCardinalDirection.Center,
                    Scale = new(1.5f),
                    Size = new(SGUI_ItemExplorerConstants.GRID_SIZE),
                };

                SGUIImageElement slotIcon = new(this.SGameInstance)
                {
                    Texture = this.paginationButtons[i].IconTexture,
                    OriginPivot = SCardinalDirection.Center,
                    Scale = new(1f),
                    Size = new(SGUI_ItemExplorerConstants.GRID_SIZE)
                };

                // Spacing
                this.paginationButtonSlots[i] = new(slotBackground, slotIcon);

                // Adding
                layoutBuilder.AddElement(slotBackground);
                layoutBuilder.AddElement(slotIcon);
            }

            // Left
            SSlot leftSlot = this.paginationButtonSlots[0];
            leftSlot.BackgroundElement.PositionAnchor = SCardinalDirection.Southwest;
            leftSlot.BackgroundElement.Margin = new(34f, -34f);

            // Right
            SSlot rightSlot = this.paginationButtonSlots[1];
            rightSlot.BackgroundElement.PositionAnchor = SCardinalDirection.Southeast;
            rightSlot.BackgroundElement.Margin = new(-34f);

            foreach (SSlot slot in this.paginationButtonSlots)
            {
                slot.BackgroundElement.PositionRelativeToElement(this.panelBackgroundElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);
            }
        }
    }
}
